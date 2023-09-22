using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RKdigitalsAPI.HtmlTemplates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RKdigitalsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPdfCreatorController : ControllerBase
    {
        private IConverter _converter;
        public UserPdfCreatorController(IConverter converter)
        {
            _converter = converter;
        }
        [HttpGet("CreateAndDownload")]
        public IActionResult CreatePDF()
        {
            var path = @"D:\Rahul Kondi\FullStack\PhotoFrameProject\API\RKdigitalsAPI\HtmlTemplates\Users.pdf";
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = path
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = UserTemplateGenerator.GetUserHtmlStirng(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Rkdigital's" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            _converter.Convert(pdf);
            return File(System.IO.File.ReadAllBytes(path), "application/pdf", "TotalUsers.pdf");
        }
        
    }
}
