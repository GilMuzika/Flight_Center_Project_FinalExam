using Flight_Center_Project_FinalExam_BL;
using Flight_Center_Project_FinalExam_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;

namespace Web_Api_interface.ApiControllers
{
    [RoutePrefix("api/Search")]
    public class SearchController : ApiController
    {
        FlyingCenterSystem _fsc = FlyingCenterSystem.GetInstance();

        AnonimousUserFacade _anonimousUserFacade;

        public SearchController()
        {
            _anonimousUserFacade = _fsc.GetAnonimousFacade();
        }



        [ResponseType(typeof(Flight))]
        [HttpGet]
        [Route("GetSearchResults", Name = "GetSearchResults")]
        public virtual IHttpActionResult GetAllFlights([FromUri]string pageKind, string searchCriterion1, string searchCriterion2)
        {
            var r = pageKind + searchCriterion1 + searchCriterion2;

            List<Flight> flights = _anonimousUserFacade.GetAllFlights();
            if (flights.Count == 0) return NotFound();

            var textResultModel = _anonimousUserFacade.Get<WebPageSearchResultsHTMLModel>(2);
            string txtResultString = string.Empty;
            txtResultString += textResultModel.ModelConstantPart;
            //txtResultString += textResultModel.ModelRepetitivePart;


            foreach(var flight in flights)
            {
                string textModelCurrent = textResultModel.ModelRepetitivePart;
                //Flight_Center_Project_FinalExam_DAL.Country departureCountry = _anonimousUserFacade.Get<Flight_Center_Project_FinalExam_DAL.Country>(flight.ORIGIN_COUNTRY_CODE);
                Country departureCountry = _anonimousUserFacade.GetSomethingInOneTableBySomethingInAnother<Flight_Center_Project_FinalExam_DAL.Country>(flight.ORIGIN_COUNTRY_CODE, "ID", "ORIGIN_COUNTRY_CODE", 2, typeof(Flight));
                //Flight_Center_Project_FinalExam_DAL.AirlineCompany airCompany = _anonimousUserFacade.Get<Flight_Center_Project_FinalExam_DAL.AirlineCompany>(flight.AIRLINECOMPANY_ID);
                AirlineCompany airCompany = _anonimousUserFacade.GetSomethingInOneTableBySomethingInAnother<Flight_Center_Project_FinalExam_DAL.AirlineCompany>(flight.AIRLINECOMPANY_ID, "ID", "AIRLINECOMPANY_ID", 1, typeof(Flight));
                //Flight_Center_Project_FinalExam_DAL.Country destinationCountry = _anonimousUserFacade.Get<Flight_Center_Project_FinalExam_DAL.Country>(flight.DESTINATION_COUNTRY_CODE);
                Country destinationCountry = _anonimousUserFacade.GetSomethingInOneTableBySomethingInAnother<Flight_Center_Project_FinalExam_DAL.Country>(flight.DESTINATION_COUNTRY_CODE, "ID", "DESTINATION_COUNTRY_CODE", 3, typeof(Flight));                

                textModelCurrent = textModelCurrent.Replace("###airCompany.ADORNING###", airCompany.ADORNING);
                textModelCurrent = textModelCurrent.Replace("###departureCountry.COUNTRY_NAME###", departureCountry.COUNTRY_NAME);
                textModelCurrent = textModelCurrent.Replace("###destinationCountry.COUNTRY_NAME###", destinationCountry.COUNTRY_NAME);
                if (airCompany.IMAGE != null && airCompany.IMAGE.Length > 50)
                    textModelCurrent = textModelCurrent.Replace("~/ViewsImages/FlightSystemMain/transparentTerminator.gif", Web_Api_interface.Views.FlightSystemMain.ImageRestorer.GetFormattedImage64baseString(airCompany.IMAGE));

                txtResultString += textModelCurrent;
            }
            


            
            return Ok(ClearString(txtResultString));
        }

        private string ClearString(string str)
        {
            str = str.Replace(" ", "___");
            Regex regex = new Regex(@"\s+");
            string txtResultString = regex.Replace(str, String.Empty);
            return txtResultString.Replace("___", " ");
        }
    }
}
