using BookStore_API.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private IConfiguration _config;
        private ITokenService _iTokenService;

        public BookController(IConfiguration config,
                              ITokenService iTokenService)
        {
            _config = config;
            _iTokenService = iTokenService;
        }

        [HttpGet]        
        public async Task<IActionResult> GetBooks()
        {
            LibraryModel library = new LibraryModel();
            List<BooksModel> books = new List<BooksModel>();
            
            //var user = AuthenticateUser(login);
            LoginModel user = new LoginModel { Username = "Faizan.Jalil", Password = "password" };
            string token = await _iTokenService.FetchTokenAsync(user);
                       
            //if (login.Username == "Faizan.Jalil")
            {
                books.Add(new BooksModel { Id = 1, Name = "book1" });
                books.Add(new BooksModel { Id = 2, Name = "book2" });
                books.Add(new BooksModel { Id = 3, Name = "book3" });
            }

            library.Token = token;
            library.Books = books;

            string json = ConvertXmlToJson();

            return Ok(library);
        }

        [HttpGet]
        [Route("GetBooksJson")]
        //[Produces("application/xml")]
        public async Task<IActionResult> GetBooksJson()
        {
            LibraryModel library = new LibraryModel();
            List<BooksModel> books = new List<BooksModel>();

            //var user = AuthenticateUser(login);
            LoginModel user = new LoginModel { Username = "Faizan.Jalil", Password = "password" };
            string token = await _iTokenService.FetchTokenAsync(user);

            //if (login.Username == "Faizan.Jalil")
            {
                books.Add(new BooksModel { Id = 1, Name = "book1" });
                books.Add(new BooksModel { Id = 2, Name = "book2" });
                books.Add(new BooksModel { Id = 3, Name = "book3" });
            }

            library.Token = token;
            library.Books = books;

            string json = ConvertXmlToJson();

            return Ok(json);
        }

        [HttpGet]
        [Route("GetBooksXmlFromJson")]
        [Produces("application/xml")]
        public async Task<IActionResult> GetBooksXmlFromJson()
        {
            LibraryModel library = new LibraryModel();
            List<BooksModel> books = new List<BooksModel>();

            //var user = AuthenticateUser(login);
            LoginModel user = new LoginModel { Username = "Faizan.Jalil", Password = "password" };
            string token = await _iTokenService.FetchTokenAsync(user);

            //if (login.Username == "Faizan.Jalil")
            {
                books.Add(new BooksModel { Id = 1, Name = "book1" });
                books.Add(new BooksModel { Id = 2, Name = "book2" });
                books.Add(new BooksModel { Id = 3, Name = "book3" });
            }

            library.Token = token;
            library.Books = books;

            string json = ConvertJsonToXml();

            return Ok(json);
        }

        private string ConvertXmlToJson()
        {

            string xml = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                                  <soap:Body>
                                    <EnvAssignMortgageAgreementIdRq xmlns='http://schemas.td.com/dxm/srm/productmanagement/lending/termlendingengine/mortgagemanager/mortgagegateway/v1'>
                                      <ManagedApp xmlns='http://schemas.td.com/dxm/mex/v1'>
                                        <IssuerId>com.td.mci.mal</IssuerId>
                                        <Code>FNF</Code>
                                        <DxmVersion>1.7</DxmVersion>
                                      </ManagedApp>
                                      <ID xmlns='http://schemas.td.com/dxm/mex/v1'>urn:uuid:fc95675e-4a71-4ef3-b517-af6be84ede31</ID>
                                      <TraceabilityID xmlns='http://schemas.td.com/dxm/mex/v1'>910ec7a2-799c-4b1b-8995-1a26c1125e27|dxm:BKR1,,1.2@com.td.dxm.mal</TraceabilityID>
                                      <SvcAssignMortgageAgreementIdRq>
                                        <MessageID xmlns='http://schemas.td.com/dxm/mex/v1'>urn:uuid:d360a773-1400-4ca4-bff1-f1b44e7739e4</MessageID>
                                        <RuleSetName>Assign</RuleSetName>
                                      </SvcAssignMortgageAgreementIdRq>
                                    </EnvAssignMortgageAgreementIdRq>
                                  </soap:Body>
                                </soap:Envelope>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string json = JsonConvert.SerializeXmlNode(doc);

            return json;
        }

        private string ConvertJsonToXml()
        {

            string json = @"{
							'token': 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MjIyMjY2NzcsImlzcyI6IlRlc3QuY29tIiwiYXVkIjoiVGVzdC5jb20ifQ.mTodGcgbMcDs-Pgv55vDgqMdqc20mlTqAtiOVyBP32A',
							'books': [
								{
									'id': 1,
									'name': 'book1'
								},
								{
									'id': 2,
									'name': 'book2'
								},
								{
									'id': 3,
									'name': 'book3'
								}
							]
						}";

            XNode node = JsonConvert.DeserializeXNode(json, "Root");

            return node.ToString();
        }

    }
}
