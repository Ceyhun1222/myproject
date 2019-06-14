using AutoMapper;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OmsApi.Data;
using OmsApi.Dto;
using OmsApi.Entity;
using OmsApi.Interfaces;
using OmsApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[Consumes("application/json")]
    [Produces("application/json")]
    public class RequestController : BaseController
    {
        private ApplicationDbContext _dbContext;
        private readonly ILogger<RequestController> _logger;
        private UserManager<ApplicationUser> _userManager;
        public RequestController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager,
            ILogger<RequestController> logger)
        {
            _dbContext = applicationDbContext;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        [SwaggerOperation("Gets all submitted requests")]
        [SwaggerResponse((int) StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<ActionResult<RequestDto4AdminList>> Get4Admin(long? userId, Guid? aerodromeId,
            string designator, string sortBy, SortOrder? order, int? page, int? perPage, [FromServices] IMapper mapper)
        {

            var (requestList, totalCount) =
                await FilterRequests(true, userId, aerodromeId, designator, sortBy, order, page, perPage);
            if (totalCount == 0)
                return Ok(new RequestDto4AdminList());
            requestList.ForEach(t =>
            {
                if (t.Status == Status.New)
                {
                    var k = new Request() {Id = t.Id};
                    _dbContext.Attach(k);
                    _dbContext.Entry(k).Property("Status").CurrentValue = Status.Pending;
                    //t.Status = Status.Pending;
                }
            });
            await _dbContext.SaveChangesAsync();
            try
            {
                return Ok(new RequestDto4AdminList()
                {
                    Data = mapper.Map<List<RequestDto4Admin>>(requestList),
                    Count = totalCount
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Tuple<List<Request>,int>> FilterRequests(bool submitted, long? userId, Guid? aerodromeId, string designator, string sortBy, SortOrder? order,
            int? page, int? perPage)
        {
            IQueryable<Request> requests = _dbContext.Requests.Include(t => t.CreatedBy).Select(t => new Request()
            {
                AirportId = t.AirportId,
                AirportName = t.AirportName,
                BeginningDate = t.BeginningDate,
                Checked = t.Checked,
                CreatedAt = t.CreatedAt,
                CreatedBy = t.CreatedBy,
                Designator = t.Designator,
                Duration = t.Duration,
                Elevation = t.Elevation,
                EndDate = t.EndDate,
                Height = t.Height,
                HorizontalAccuracy = t.HorizontalAccuracy,
                Id = t.Id,
                Identifier = t.Identifier,
                ObstructionType = t.ObstructionType,
                Status = t.Status,
                Submitted = t.Submitted,
                Submitted2Aixm = t.Submitted2Aixm,
                Submitted2AixmAt = t.Submitted2AixmAt,
                Submitted2AixmPrivateSlotId = t.Submitted2AixmPrivateSlotId,
                Submitted2AixmPrivateSlotName = t.Submitted2AixmPrivateSlotName,
                Submitted2AixmPublicSlotId = t.Submitted2AixmPublicSlotId,
                Submitted2AixmPublicSlotName = t.Submitted2AixmPublicSlotName,
                Type = t.Type,
                VerticalAccuracy = t.VerticalAccuracy,
                Geometry = t.Geometry
            });
            if (submitted)
                requests = requests.Where(t => t.Submitted);
                
            if (userId.HasValue)
            {
                requests = requests.Where(t => t.CreatedBy.Id == userId);
            }

            if (aerodromeId.HasValue)
            {
                requests = requests.Where(t => t.AirportId == aerodromeId.ToString());
            }

            if (!string.IsNullOrEmpty(designator))
            {
                requests = requests.Where(t =>
                    t.Designator.Contains(designator, StringComparison.InvariantCultureIgnoreCase));
            }

            var totalCount = await requests.CountAsync();
            if (totalCount == 0)
            {
                return new Tuple<List<Request>, int>(null, 0);
            }

            if (!perPage.HasValue || perPage.Value < 0)
                perPage = 10;
            if (!page.HasValue || page.Value < 1)
                page = 1;
            var skip = ((page.Value - 1) * perPage.Value);
            if (totalCount <= skip)
            {
                skip = 0;
            }

            var remainder = totalCount - (perPage.Value + skip);
            var take = remainder < 0 ? totalCount - skip : perPage;
            var stop = new Stopwatch();
            //stop.Start();
            //var lstFull = await SortBy<Request>(requests, sortBy,
            //    order ?? SortOrder.Desc).ToListAsync();
            //stop.Stop();
            //var t1 = stop.Elapsed;
            stop.Restart();
            var lst = await SortBy<Request>(requests, sortBy,
                order ?? SortOrder.Desc).Skip(skip).Take(take.Value).ToListAsync();
            stop.Stop();
            var t2 = stop.Elapsed;
            return new Tuple<List<Request>, int>(lst, totalCount);
        }

        public IQueryable<T> SortBy<T>(IQueryable<T> source,
            string propertyName,
            SortOrder direction)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (String.IsNullOrEmpty(propertyName))
                propertyName = nameof(OmsApi.Entity.Request.CreatedAt);

            // Create a parameter to pass into the Lambda expression
            //(Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(T), "Entity");

            //  create the selector part, but support child properties (it works without . too)
            String[] childProperties = propertyName.Split('.');
            MemberExpression property = Expression.Property(parameter, childProperties[0]);
            for (int i = 1; i < childProperties.Length; i++)
            {
                property = Expression.Property(property, childProperties[i]);
            }

            LambdaExpression selector = Expression.Lambda(property, parameter);

            string methodName = (direction == SortOrder.Desc) ? "OrderByDescending" : "OrderBy";

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                new Type[] { source.ElementType, property.Type },
                source.Expression, Expression.Quote(selector));

            return source.Provider.CreateQuery<T>(resultExp);
        }

        [HttpGet("Mine")]
        [SwaggerOperation("Gets all my requests")]
        [SwaggerResponse((int) StatusCodes.Status200OK)]
        public async Task<ActionResult<RequestDto4ClientList>> Get4Client(Guid? aerodromeId, string designator,
            string sortBy, SortOrder? order, int? page, int? perPage, [FromServices] IMapper mapper)
        {
            var user = await GetUser(_userManager);
            _logger.LogInformation($"Current user is {user.UserName}");
            var (requestList, totalCount) =
                await FilterRequests(false, user.Id, aerodromeId, designator, sortBy, order, page, perPage);
            if (totalCount == 0)
                return Ok(new RequestDto4ClientList());

            return Ok(new RequestDto4ClientList()
            {
                Data = mapper.Map<IList<RequestDto4Client>>(requestList),
                Count = totalCount
            });
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Gets specified request")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<ActionResult<RequestDto4Admin>> Get4Admin(int id, [FromServices] IMapper mapper)
        {
            var user = await GetUser(_userManager);
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).Where(t => t.Id == id).FirstOrDefaultAsync();
            var k = mapper.Map<RequestDto4Admin>(request);
            return Ok(k);
        }

        [HttpGet("Mine/{id}")]
        [SwaggerOperation("Gets specified request")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RequestDto4Client>> Get4Client(int id, [FromServices] IMapper mapper)
        {
            var user = await GetUser(_userManager);
            _logger.LogInformation($"Current user is {user.UserName}");
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).Where(t => t.Id == id).FirstOrDefaultAsync();            
            if (await _userManager.IsInRoleAsync(user, Roles.Client.ToString()))
            {
                if (request.CreatedBy.Id != user.Id)
                    return Forbid();
            }
            var k = mapper.Map<RequestDto4Client>(request);
            return Ok(k);
        }

        [HttpGet("GeoJson/{id}")]
        [SwaggerOperation("Gets specified request in geojson format")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status403Forbidden, "Request has not submitted yet or you're not the owner/admin")]
        public async Task<ActionResult<FeatureCollection>> GetGeoJson(long id, [FromServices] IMapper mapper)
        {
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).FirstOrDefaultAsync(t => t.Id == id);
            if (request == null)
                return BadRequest(NotFound());
            var user = await GetUser(_userManager);
            if (!request.Submitted || (await _userManager.IsInRoleAsync(user, Roles.Client.ToString()) && request.CreatedBy.Id != user.Id))
            {
                return Forbid();
            }

            var result = new FeatureCollection();
            var geometry = JsonConvert.DeserializeObject<IGeometryObject>(request.Geometry, new GeometryConverter());
            var dictionary = new Dictionary<string, object>
            {
                { nameof(request.Type), request.Type },
                { nameof(request.Elevation), request.Elevation },
                { nameof(request.BeginningDate), request.BeginningDate },
                { nameof(request.EndDate), request.EndDate }
            };
            //if (request.Duration == Duration.Temporary)
            //    dictionary.Add(nameof(request.TemporaryEndDate), request.TemporaryEndDate);
            dictionary.Add(nameof(request.Height), request.Height);
            var feat = new Feature(geometry, dictionary, request.Id.ToString());
            result.Features.Add(feat);
            return Ok(result);
        }

        [HttpGet("Attachment/{id}")]
        [SwaggerOperation("Gets attachment of specified request")]
        [Authorize(Roles = nameof(Roles.Admin))]
        [SwaggerResponse((int) StatusCodes.Status200OK)]
        [SwaggerResponse((int) StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Attachment(long id)
        {
            var user = await GetUser(_userManager);
            var request = await _dbContext.Requests.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest(NotFound());
            var str = Encoding.ASCII.GetString(request.Attachment);
            return Ok(str);
        }

        [HttpGet("NotificationCount")]
        [SwaggerOperation("Gets request notifications count")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotificationCount()
        {
            var count = await _dbContext.Requests.CountAsync(t => t.Status == Status.New && t.Submitted);
            return Ok(count);
        }

        [HttpGet("ReportData/{requestId}")]
        [SwaggerOperation("Returns request reports for checked request")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<RequestReportDto>>> GetReportData(long requestId, [FromServices] IMapper mapper)
        {
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).Where(t => t.Id == requestId).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest(NotFound());
            if (!request.Checked)
                return BadRequest("Not checked");
            var reports = await _dbContext.RequestReports.Include(t => t.Request).Where(t => t.Request.Id == request.Id).ToListAsync();
            var resultList = mapper.Map<IList<RequestReportDto>>(reports);
            return Ok(resultList);
        }

        [HttpGet("DownloadReport/{id}")]
        [SwaggerOperation("Downloads fully report for specified request")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<ActionResult> DownloadReport(long id, [FromServices] IHostingEnvironment hostingEnvironment, [FromServices] IMapper mapper)
        {
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).FirstOrDefaultAsync(t => t.Id == id);
            if (request == null)
            {
                _logger.LogInformation($"Request not found");
                return BadRequest(NotFound());
            }
            _logger.LogInformation($"Starting to generate report");
            var pdfByreArray = await GenerateReportInPdf(request, hostingEnvironment, mapper);
            _logger.LogInformation($"Generated reports size is {pdfByreArray.Length / 1024} KB");
            return File(pdfByreArray, "application/pdf", request.Designator);
        }

        private async Task<byte[]> GenerateReportInPdf(Request request, IHostingEnvironment hostingEnvironment, IMapper mapper)
        {
            var document = new Document();
            byte[] result;
            using (var stream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();
                document.NewPage();

                // Title
                var table = new PdfPTable(1)
                {
                    WidthPercentage = 100
                };

                var phrase = new Phrase(new Chunk("Obstacle Management System", FontFactory.GetFont("Arial", 16, Font.NORMAL, BaseColor.RED)));
                var cell = PhraseCell(phrase, Element.ALIGN_CENTER);
                table.AddCell(cell);

                //Separator line
                DrawLine(writer, 25f, document.Top - 29f, document.PageSize.Width - 25f, document.Top - 29f, BaseColor.LIGHT_GRAY);
                DrawLine(writer, 25f, document.Top - 30f, document.PageSize.Width - 25f, document.Top - 30f, BaseColor.LIGHT_GRAY);
                document.Add(table);

                #region Request details

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    SpacingBefore = 50f
                };
                table.SetWidths(new float[] { 0.25f, 0.75f });

                phrase = new Phrase(new Chunk("Request details", FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLACK)));
                cell = PhraseCell(phrase, Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 20;
                table.AddCell(cell);

                _logger.LogInformation(
                    $"Mapping is calling on request which was created by {request.CreatedBy.Firstname}");

                var reqDto = mapper.Map<RequestRegistrationDto>(request);
                CreateRow("Airport", $"{reqDto.AirportName}", table);
                CreateRow("Created by", $"{request.CreatedBy.Firstname} {request.CreatedBy.Lastname}", table);
                CreateRow("Created at", $"{request.CreatedAt.ToShortDateString()}", table);
                CreateRow("Type", reqDto.Type.ToString(), table);
                CreateRow("Designator", reqDto.Designator, table);
                CreateRow("Begging date", reqDto.BeginningDate.ToShortDateString(), table);
                CreateRow("End date", reqDto.EndDate.ToShortDateString(), table);
                CreateRow("Duration", reqDto.Duration.ToString(), table);
                CreateRow("Site elevation", $"{reqDto.Elevation} M", table);
                CreateRow("Height", $"{reqDto.Height} M", table);
                CreateRow("Elevation", $"{request.Elevation} M", table);
                CreateRow("Vertical Accuracy", $"{reqDto.VerticalAccuracy} M", table);
                CreateRow("Horizontal Accuracy", $"{reqDto.HorizontalAccuracy} M", table);
                CreateRow("Obstruction type", $"{reqDto.ObstructionType.ToString()}", table);
                CreateRow("Geometry type", $"{reqDto.GeometryType.ToString()}", table);
                _logger.LogInformation($"Coordinates count is {reqDto.Coordinates.Count}");
                var x = DD2DMS(reqDto.Coordinates[0].Longitude, true, 1);
                var y = DD2DMS(reqDto.Coordinates[0].Latitude, false, 1);
                _logger.LogInformation($"First point : X = {x}; Y {y}");
                CreateRow("Coordinates", $"{x}     {y}", table);
                if (reqDto.Coordinates.Count > 1)
                {
                    for (var i = 1; i < reqDto.Coordinates.Count; i++)
                    {
                        x = DD2DMS(reqDto.Coordinates[i].Longitude, true, 1);
                        y = DD2DMS(reqDto.Coordinates[i].Latitude, false, 1);
                        CreateRow("", $"{x}     {y}", table); ;
                        _logger.LogInformation($"Next point : X = {x}; Y {y}");
                    }
                }

                document.Add(table);

                #endregion

                #region Reports details

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                    SpacingBefore = 50f
                };

                phrase = new Phrase(new Chunk("Report details", FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLACK)));
                cell = PhraseCell(phrase, Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 20;
                table.AddCell(cell);



                var foundReports = await _dbContext.RequestReports.Where(t => t.Request == request).ToListAsync();
                _logger.LogInformation($"Report count is {foundReports.Count}");
                var surfaceName = string.Empty;
                const string removedKeyWord = "OTHER_";
                foreach (var report in foundReports)
                {
                    CreateRow("Surface name", report.SurfaceName.Substring(removedKeyWord.Length), table);
                    CreateRow("Penetrate", $"{report.Penetrate} M", table);
                    CreateRow("Runway direction", report.RunwayDirection, table);
                    cell = PhraseCell(new Phrase("\n", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
                document.Add(table);

                #endregion

                #region Attachcment
                _logger.LogInformation($"Attachment length is {request.Attachment?.Length}");
                if (request.Attachment?.Length > 0)
                {
                    do
                    {
                        document.Add(new Chunk());
                    } while (!document.NewPage());

                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                        SpacingBefore = 50f
                    };

                    phrase = new Phrase(new Chunk("Attachment", FontFactory.GetFont("Arial", 12, Font.UNDERLINE, BaseColor.BLACK)));
                    cell = PhraseCell(phrase, Element.ALIGN_CENTER);
                    cell.Colspan = 2;
                    cell.PaddingBottom = 20;
                    table.AddCell(cell);
                    _logger.LogInformation($"Attachment length is {request.Attachment.Length}");
                    try
                    {
                        var k = Encoding.ASCII.GetString(request.Attachment).Substring(@"data:image/jpeg;base64,".Length);
                        var img = Image.GetInstance(Convert.FromBase64String(k));
                        _logger.LogInformation($"Converted to image");
                        img.ScaleToFit(500, 300);
                        var imageCell = ImageCell(img, 0, Element.ALIGN_CENTER);
                        table.AddCell(imageCell);
                        document.Add(table);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                #endregion

                document.Close();
                result = stream.ToArray();
                stream.Close();
            }
            return result;
        }

        private string DD2DMS(double xORy, bool isX, int round)
        {
            double k = xORy;
            double deg, min, sec;
            int sign = Math.Sign(k);

            {
                double n = Math.Abs(Math.Round(Math.Abs(k) * sign, 10));

                deg = (int)n;
                double dn = (n - deg) * 60;
                dn = Math.Round(dn, 8);
                min = (int)dn;
                sec = (dn - min) * 60;
            }

            string degStr = deg.ToString();
            int strLen = 2;
            string signSymb = "SN";

            if (isX)
            {
                strLen = 3;
                signSymb = "WE";
            }

            sec = Math.Round(sec, round);

            while (degStr.Length < strLen)
                degStr = "0" + degStr;

            return string.Format("{0}°{1}'{2}\" {3}", degStr, min, sec, signSymb[(sign + 1) >> 1]);
        }

        private void CreateRow(string key, string value, PdfPTable table)
        {
            table.AddCell(PhraseCell(new Phrase($"{key}", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
            table.AddCell(PhraseCell(new Phrase(value, FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
            var cell = PhraseCell(new Phrase("\n", FontFactory.GetFont("Arial", 6, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
            cell.Colspan = 2;
            table.AddCell(cell);
        }

        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }
        private static PdfPCell ImageCell(Image image, float scale, int align)
        {
            //image.ScalePercent(scale);
            PdfPCell cell = new PdfPCell(image);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }

        [HttpGet("ReportTree")]
        [SwaggerOperation("Returns hiearchy of layers")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RequestTreeViewDto>> Report(long id, [FromServices] ITossClient tossClient)
        {
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).Where(t => t.Id == id).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest(NotFound());
            var slot = await _dbContext.GetSelectedSlot();
            var result = await tossClient.GetObstacleAreaDtos(slot.Private.TossId, request.AirportId, _logger);
            await FindAppropriateSurfaces(result, request);
            result.AirportName = request.AirportName;
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation("Creates request")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Post(RequestRegistrationDto value, [FromServices] IMapper mapper)
        {
            try
            {
                var user = await GetUser(_userManager);
                var request = mapper.Map<Request>(value);
                request.CreatedBy = user;
                _dbContext.Requests.Add(request);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                var k = ex.Message;
                throw;
            }
        }

        [HttpPost("Submit/{id}")]
        [SwaggerOperation("Submits request")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status403Forbidden, "Only owner can submit")]
        public async Task<IActionResult> Submit(long id, [FromServices] IMapper mapper,
            [FromServices] IOmsEmailSender emailSender)
        {
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).FirstOrDefaultAsync(k => k.Id == id);
            if (request == null)
                return BadRequest(NotFound());
            var user = await GetUser(_userManager);
            if (request.CreatedBy.Id == user.Id && !request.Submitted)
            {
                request.Submitted = true;
                request.Identifier = Guid.NewGuid();
                await _dbContext.SaveChangesAsync();
                await emailSender.Send2AdminSignupMessage(user.UserName, $"{user.Firstname} {user.Lastname}",_logger);
                return NoContent();
            }
            return Forbid();
        }

        [HttpPost("Submit2Aixm/{requestId}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        [SwaggerOperation("Submits request and relatd reports to the main AIXM 5.1 DB")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Submit2Aixm(long requestId, [FromServices] ITossClient tossClient, [FromServices] IMapper mapper)
        {
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).FirstOrDefaultAsync(k => k.Id == requestId);
            if (request == null)
                return BadRequest(NotFound());
            var user = await GetUser(_userManager);
            var slot = await _dbContext.GetSelectedSlot();

            var vertStructure = mapper.Map<TossRequestSubmit2AixmDto>(request);
            var reports = await _dbContext.RequestReports.Include(t => t.Request).Where(t => t.Request.Id == request.Id).ToListAsync();
            vertStructure.ObstacleAreadIdList = reports.Select(t => t.SurfaceIdentifier).ToList();
            var response = await tossClient.Submit2Aixm(slot.Private.TossId, vertStructure);
            if (!string.IsNullOrEmpty(response))
                return BadRequest(response);
            request.Submitted2Aixm = true;
            request.Submitted2AixmPrivateSlotId = slot.Private.TossId;
            request.Submitted2AixmPrivateSlotName = slot.Private.Name;
            request.Submitted2AixmPublicSlotId = slot.TossId;
            request.Submitted2AixmAt = DateTime.Now;
            request.Submitted2AixmPublicSlotName = slot.Name;
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("Check/{requestId}")]
        [SwaggerOperation("Checks specified request for the penetration")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IList<RequestReportDto>>> Check(long requestId, [FromServices] IOmegaServiceClient omegaServiceClient, [FromServices] IMapper mapper)
        {
            var request = await _dbContext.Requests.Include(t => t.CreatedBy).Where(t => t.Id == requestId).FirstOrDefaultAsync();
            _logger.LogInformation($"Request is created by {request.CreatedBy.Firstname} {request.CreatedBy.Lastname} at {request.CreatedBy.LastModifiedAt}");
            if (request == null)
                return BadRequest(NotFound());
            var slot = await _dbContext.GetSelectedSlot();
            var requestRegistry = mapper.Map<RequestRegistrationDto>(request);
            var requestData = new RequestCheckDto()
            {
                AdhpIdentifier = request.AirportId,
                Elevation = request.Elevation,
                GeometryType = (int)requestRegistry.GeometryType,
                HorizontalAccuracy = request.HorizontalAccuracy,
                VerticalAccuracy = request.VerticalAccuracy,
                WorkPackage = (int)slot.Private.TossId,
                Points = requestRegistry.Coordinates,
                Height = requestRegistry.Height
            };
            var result = await omegaServiceClient.CheckRequest(requestData, _logger);

            var keyword = ObstacleAreaType.OTHER_OUTERHORIZONTAL.ToString();
            RemoveDuplicates(result, keyword);
            keyword = ObstacleAreaType.OTHER_INNERHORIZONTAL.ToString();
            RemoveDuplicates(result, keyword);
            keyword = ObstacleAreaType.OTHER_CONICAL.ToString();
            RemoveDuplicates(result, keyword);

            var resultReportList = mapper.Map<List<RequestReport>>(result);

            var oldReportList = await _dbContext.RequestReports.Include(t => t.Request).Where(t => t.Request.Id == request.Id).ToListAsync();
            _dbContext.RequestReports.RemoveRange(oldReportList);
            resultReportList.ForEach(t => t.Request = request);
            request.Checked = true;
            await _dbContext.RequestReports.AddRangeAsync(resultReportList);
            await _dbContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPut]
        [SwaggerOperation("Updates specified request")]
        [SwaggerResponse((int)StatusCodes.Status403Forbidden, "Can't update not submitted request")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(RequestRegistrationDto registrationDto, [FromServices] IMapper mapper)
        {
            var request = await _dbContext.Requests.Where(t => t.Id == registrationDto.Id).FirstOrDefaultAsync();
            if (request != null && request.Submitted)
            {
                return Forbid();
            }
            request = mapper.Map<RequestRegistrationDto, Request>(registrationDto, request);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Deletes specified request")]
        [SwaggerResponse((int)StatusCodes.Status403Forbidden, "Only owner or admin can delete it")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            var request = _dbContext.Requests.Include(t => t.CreatedBy).FirstOrDefault(k => k.Id == id);
            if (request == null)
                return BadRequest(NotFound());
            var user = await GetUser(_userManager);
            if (await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()) || request.CreatedBy.Id == user.Id)
            {
                _dbContext.RemoveRange(_dbContext.RequestReports.Include(t => t.Request).Where(t => t.Request.Id == id));
                _dbContext.Remove(request);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            return Forbid();
        }

        private async Task FindAppropriateSurfaces(RequestTreeViewDto requestTreeViewDto, Request request)
        {
            _logger.LogTrace($"Runway count of RequestTreeView is {requestTreeViewDto.Runways.Count}");
            var foundReports = await _dbContext.RequestReports.Where(t => t.Request == request).ToListAsync();
            _logger.LogTrace($"Found report count is {foundReports?.Count}");
            foreach (var runway in requestTreeViewDto.Runways)
            {
                _logger.LogTrace($"RunwayDirection count of Runway ({runway.Name}) is {runway.RunwayDirections.Count}");
                foreach (var runwayDirection in runway.RunwayDirections)
                {
                    _logger.LogTrace($"ObstacleArea count of RunwayDirection ({runwayDirection?.Name}) is {runwayDirection?.ObstacleAreas?.Count}");
                    if (runwayDirection.ObstacleAreas != null)
                    {
                        foreach (var area in runwayDirection.ObstacleAreas)
                        {
                            area.IsChecked = foundReports.Any(t => t.SurfaceIdentifier.Equals(area.Id));
                            _logger.LogTrace($"IsChecked value of {area.Name}({area.Id}) is {area.IsChecked}");
                        }
                    }
                }
            }
            _logger.LogTrace($"ObstacleArea count of RequestTreeView is {requestTreeViewDto.ObstacleAreas.Count}");
            if (requestTreeViewDto.ObstacleAreas != null)
            {
                foreach (var area in requestTreeViewDto.ObstacleAreas)
                {
                    area.IsChecked = foundReports.Any(t => t.SurfaceIdentifier.Equals(area.Id));
                    _logger.LogTrace($"IsChecked value of {area.Name}({area.Id}) is {area.IsChecked}");
                }
            }
        }

        private void RemoveDuplicates(IList<RequestReportDto> reportList, string keyword)
        {
            RequestReportDto searchFor = null;
            double epislon = 0.001;
            var result = new List<RequestReportDto>();
            foreach (var item in reportList)
            {
                if (string.Equals(item.SurfaceName, keyword))
                {
                    if (searchFor == null)
                    {
                        searchFor = item;
                        continue;
                    }
                    if (Math.Abs(item.Penetrate - searchFor.Penetrate) <= epislon)
                    {
                        result.Add(item);
                    }
                }
            }
            result.ForEach(t => reportList.Remove(t));
        }
    }
}