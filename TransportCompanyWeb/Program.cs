using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TransportCompanyWeb.Data;
using TransportCompanyWeb.Service;
using Microsoft.AspNetCore.Http;

namespace TransportCompanyWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ������ ������ ����������� �� appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DBConnection");

            // ����������� ��������
            builder.Services.AddDbContext<TransportCompanyContext>(options =>
                options.UseSqlServer(connectionString));

            // ����������� ����������� � ������
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<CachedDataService>();

            // ����������� ������
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.UseSession();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.ContentType = "text/html; charset=utf-8"; 
                    string strResponse = "<HTML><HEAD><TITLE>������� ��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY>";
                    strResponse += "<BR><A href='/table'>�������</A>";
                    strResponse += "<BR><A href='/info'>����������</A>";
                    strResponse += "<BR><A href='/searchform1'>SearchForm1</A>";
                    strResponse += "<BR><A href='/searchform2'>SearchForm2</A>";
                    strResponse += "</BODY></HTML>";
                    await context.Response.WriteAsync(strResponse);
                    return;
                }
                await next.Invoke();
            });

            app.Map("/searchform1", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8"; 
                    var dbContext = context.RequestServices.GetService<TransportCompanyContext>();

                    if (context.Request.Method == "GET")
                    {
                        var registrationNumber = context.Request.Cookies["RegistrationNumber"] ?? "";
                        var selectedBrandId = context.Request.Cookies["BrandId"] ?? "";
                        var selectedCarTypeIds = context.Request.Cookies["CarTypeIds"] ?? "";

                        var brands = await dbContext.CarBrands.ToListAsync();
                        var carTypes = await dbContext.CarTypes.ToListAsync();

                        var html = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Search Form 1</title></head><body>";
                        html += "<h1>Search Cars (Cookie)</h1>";
                        html += "<form method='post'>";
                        html += "<label for='RegistrationNumber'>Registration Number:</label><br/>";
                        html += $"<input type='text' id='RegistrationNumber' name='RegistrationNumber' value='{registrationNumber}' /><br/><br/>";

                        html += "<label for='BrandId'>Brand:</label><br/>";
                        html += "<select id='BrandId' name='BrandId'>";
                        foreach (var brand in brands)
                        {
                            var selected = brand.Id.ToString() == selectedBrandId ? "selected" : "";
                            html += $"<option value='{brand.Id}' {selected}>{brand.Name}</option>";
                        }
                        html += "</select><br/><br/>";

                        html += "<label for='CarTypeIds'>Car Types:</label><br/>";
                        html += "<select id='CarTypeIds' name='CarTypeIds' multiple size='3'>";
                        var selectedCarTypeIdsArray = selectedCarTypeIds.Split(',');

                        foreach (var carType in carTypes)
                        {
                            var selected = selectedCarTypeIdsArray.Contains(carType.Id.ToString()) ? "selected" : "";
                            html += $"<option value='{carType.Id}' {selected}>{carType.Name}</option>";
                        }
                        html += "</select><br/><br/>";

                        html += "<button type='submit'>Search</button>";
                        html += "</form>";
                        html += "</body></html>";

                        await context.Response.WriteAsync(html);
                    }
                    else if (context.Request.Method == "POST")
                    {
                        var formData = await context.Request.ReadFormAsync();

                        var registrationNumber = formData["RegistrationNumber"];
                        var brandId = formData["BrandId"];
                        var carTypeIds = formData["CarTypeIds"];

                        context.Response.Cookies.Append("RegistrationNumber", registrationNumber);
                        context.Response.Cookies.Append("BrandId", brandId);
                        context.Response.Cookies.Append("CarTypeIds", string.Join(",", carTypeIds));

                        var query = dbContext.Cars.Include(c => c.Brand).Include(c => c.CarType).AsQueryable();

                        if (!string.IsNullOrEmpty(registrationNumber))
                        {
                            query = query.Where(c => c.RegistrationNumber.Contains(registrationNumber));
                        }

                        if (int.TryParse(brandId, out int brandIdValue))
                        {
                            query = query.Where(c => c.BrandId == brandIdValue);
                        }

                        if (carTypeIds.Count > 0)
                        {
                            var carTypeIdValues = carTypeIds.Select(id => int.Parse(id)).ToList();
                            query = query.Where(c => carTypeIdValues.Contains(c.CarTypeId.Value));
                        }

                        var results = await query.ToListAsync();

                        var html = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Search Results</title></head><body>";
                        html += "<h1>Search Results</h1>";

                        if (results.Count > 0)
                        {
                            html += "<table border='1' style='border-collapse:collapse'>";
                            html += "<tr><th>ID</th><th>Registration Number</th><th>Brand</th><th>Car Type</th></tr>";
                            foreach (var car in results)
                            {
                                html += "<tr>";
                                html += $"<td>{car.Id}</td>";
                                html += $"<td>{car.RegistrationNumber}</td>";
                                html += $"<td>{car.Brand?.Name}</td>";
                                html += $"<td>{car.CarType?.Name}</td>";
                                html += "</tr>";
                            }
                            html += "</table>";
                        }
                        else
                        {
                            html += "<p>No results found.</p>";
                        }

                        html += "<br/><a href='/searchform1'>Back to Search</a>";
                        html += "</body></html>";

                        await context.Response.WriteAsync(html);
                    }
                });
            });

            // ��������� ���� "/searchform2" � �������������� ������
            app.Map("/searchform2", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    var dbContext = context.RequestServices.GetService<TransportCompanyContext>();

                    if (context.Request.Method == "GET")
                    {
                        var registrationNumber = context.Session.GetString("RegistrationNumber") ?? "";
                        var selectedBrandId = context.Session.GetString("BrandId") ?? "";
                        var selectedCarTypeIds = context.Session.GetString("CarTypeIds") ?? "";

                        var brands = await dbContext.CarBrands.ToListAsync();
                        var carTypes = await dbContext.CarTypes.ToListAsync();

                        var html = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Search Form 2</title></head><body>";
                        html += "<h1>Search Cars (Session)</h1>";
                        html += "<form method='post'>";
                        html += "<label for='RegistrationNumber'>Registration Number:</label><br/>";
                        html += $"<input type='text' id='RegistrationNumber' name='RegistrationNumber' value='{registrationNumber}' /><br/><br/>";

                        html += "<label for='BrandId'>Brand:</label><br/>";
                        html += "<select id='BrandId' name='BrandId'>";
                        foreach (var brand in brands)
                        {
                            var selected = brand.Id.ToString() == selectedBrandId ? "selected" : "";
                            html += $"<option value='{brand.Id}' {selected}>{brand.Name}</option>";
                        }
                        html += "</select><br/><br/>";

                        html += "<label for='CarTypeIds'>Car Types:</label><br/>";
                        html += "<select id='CarTypeIds' name='CarTypeIds' multiple size='3'>";
                        var selectedCarTypeIdsArray = selectedCarTypeIds.Split(',');

                        foreach (var carType in carTypes)
                        {
                            var selected = selectedCarTypeIdsArray.Contains(carType.Id.ToString()) ? "selected" : "";
                            html += $"<option value='{carType.Id}' {selected}>{carType.Name}</option>";
                        }
                        html += "</select><br/><br/>";

                        html += "<button type='submit'>Search</button>";
                        html += "</form>";
                        html += "</body></html>";

                        await context.Response.WriteAsync(html);
                    }
                    else if (context.Request.Method == "POST")
                    {
                        var formData = await context.Request.ReadFormAsync();

                        var registrationNumber = formData["RegistrationNumber"];
                        var brandId = formData["BrandId"];
                        var carTypeIds = formData["CarTypeIds"];

                        context.Session.SetString("RegistrationNumber", registrationNumber);
                        context.Session.SetString("BrandId", brandId);
                        context.Session.SetString("CarTypeIds", string.Join(",", carTypeIds));

                        var query = dbContext.Cars.Include(c => c.Brand).Include(c => c.CarType).AsQueryable();

                        if (!string.IsNullOrEmpty(registrationNumber))
                        {
                            query = query.Where(c => c.RegistrationNumber.Contains(registrationNumber));
                        }

                        if (int.TryParse(brandId, out int brandIdValue))
                        {
                            query = query.Where(c => c.BrandId == brandIdValue);
                        }

                        if (carTypeIds.Count > 0)
                        {
                            var carTypeIdValues = carTypeIds.Select(id => int.Parse(id)).ToList();
                            query = query.Where(c => carTypeIdValues.Contains(c.CarTypeId.Value));
                        }

                        var results = await query.ToListAsync();

                        // ������������ HTML ��� ������ �����������
                        var html = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Search Results</title></head><body>";
                        html += "<h1>Search Results</h1>";

                        if (results.Count > 0)
                        {
                            html += "<table border='1' style='border-collapse:collapse'>";
                            html += "<tr><th>ID</th><th>Registration Number</th><th>Brand</th><th>Car Type</th></tr>";
                            foreach (var car in results)
                            {
                                html += "<tr>";
                                html += $"<td>{car.Id}</td>";
                                html += $"<td>{car.RegistrationNumber}</td>";
                                html += $"<td>{car.Brand?.Name}</td>";
                                html += $"<td>{car.CarType?.Name}</td>";
                                html += "</tr>";
                            }
                            html += "</table>";
                        }
                        else
                        {
                            html += "<p>No results found.</p>";
                        }

                        html += "<br/><a href='/searchform2'>Back to Search</a>";
                        html += "</body></html>";

                        await context.Response.WriteAsync(html);
                    }
                });
            });


            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/table")
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    string strResponse = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
                     "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                     "<BODY>";
                    strResponse += "<BR><A href='/table/Cars'>Cars</A>";
                    strResponse += "<BR><A href='/table/CarBrands'>CarBrands</A>";
                    strResponse += "<BR><A href='/table/Cargos'>Cargos</A>";
                    strResponse += "<BR><A href='/table/CargoTypes'>CargoTypes</A>";
                    strResponse += "<BR><A href='/table/CarTypes'>CarTypes</A>";
                    strResponse += "<BR><A href='/table/Employees'>Employees</A>";
                    strResponse += "<BR><A href='/table/Trips'>Trips</A>";
                    strResponse += "</BODY></HTML>";
                    await context.Response.WriteAsync(strResponse);
                    return;
                }
                await next.Invoke();
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/table", out var remainingPath) && remainingPath.HasValue && remainingPath.Value.StartsWith("/"))
                {
                    context.Response.ContentType = "text/html; charset=utf-8"; // ��������� Content-Type
                    var tableName = remainingPath.Value.Substring(1); // ������� ��������� ����

                    var cachedService = context.RequestServices.GetService<CachedDataService>();

                    if (tableName == "Cars")
                    {
                        var list = cachedService.GetCars();
                        await RenderTable(context, list);
                    }
                    else if (tableName == "CarBrands")
                    {
                        var list = cachedService.GetCarBrands();
                        await RenderTable(context, list);
                    }
                    else if (tableName == "Cargos")
                    {
                        var list = cachedService.GetCargos();
                        await RenderTable(context, list);
                    }
                    else if (tableName == "CargoTypes")
                    {
                        var list = cachedService.GetCargoTypes();
                        await RenderTable(context, list);
                    }
                    else if (tableName == "CarTypes")
                    {
                        var list = cachedService.GetCarTypes();
                        await RenderTable(context, list);
                    }
                    else if (tableName == "Employees")
                    {
                        var list = cachedService.GetEmployees();
                        await RenderTable(context, list);
                    }
                    else if (tableName == "Trips")
                    {
                        var list = cachedService.GetTrips();
                        await RenderTable(context, list);
                    }
                    else
                    {
                        // ���� ������� �� �������, ���������� 404
                        context.Response.StatusCode = 404;
                        await context.Response.WriteAsync("������� �� �������");
                    }

                    return; // ��������� ��������� �������
                }
                await next.Invoke();
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/info")
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>����������:</H1>";
                    strResponse += "<BR> ������: " + context.Request.Host;
                    strResponse += "<BR> ����: " + context.Request.Path;
                    strResponse += "<BR> ��������: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";
                    await context.Response.WriteAsync(strResponse);
                    return;
                }
                await next.Invoke();
            });

            async Task RenderTable<T>(HttpContext context, IEnumerable<T> data)
            {
                context.Response.ContentType = "text/html; charset=utf-8"; 
                var html = "<table border='1' style='border-collapse:collapse'>";

                var type = typeof(T);

                // ��������� ���������� ������� �� ������ ������� ����
                html += "<tr>";
                foreach (var prop in type.GetProperties())
                {
                    // ���������� ��������, ������� �������� ��������� ������ ������� ��� �����������
                    if (!IsSimpleType(prop.PropertyType))
                    {
                        continue;
                    }

                    html += $"<th>{prop.Name}</th>";
                }
                html += "</tr>";

                foreach (var item in data)
                {
                    html += "<tr>";
                    foreach (var prop in type.GetProperties())
                    {
                        if (!IsSimpleType(prop.PropertyType))
                        {
                            continue;
                        }

                        var value = prop.GetValue(item);

                        if (value is DateTime dateValue)
                        {
                            html += $"<td>{dateValue.ToString("dd.MM.yyyy")}</td>";
                        }
                        else
                        {
                            html += $"<td>{value}</td>";
                        }
                    }
                    html += "</tr>";
                }

                html += "</table>";
                await context.Response.WriteAsync(html);
            }

            bool IsSimpleType(Type type)
            {
                // ����������� ���� � ����, ������� ��������� �������� (string, DateTime � �.�.)
                return type.IsPrimitive ||
                       type.IsValueType ||
                       type == typeof(string) ||
                       type == typeof(DateTime) ||
                       type == typeof(decimal);
            }

            app.Run();
        }
    }
}