using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System.IO;

namespace GabTrans.Application.Services
{
    public class TestService(ILogService logService, IFileService fileService, ISignUpRepository signUpRepository, ICountryRepository countryRepository, IWebHostEnvironment hostingEnvironment) : ITestService
    {
        private readonly ILogService _logService = logService;
        private readonly IFileService _fileService = fileService;
        private readonly ISignUpRepository _signUpRepository = signUpRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

        public async Task UploadCountries()
        {
            string path = "C:\\Users\\MY SPECTRE\\Desktop\\Test\\Test1";
            //string destinationDirectory = "C:\\Users\\gfalade\\Desktop\\Upload\\Processed";
            //string[] fileArray = Directory.GetFiles(sourceDirectory, "*.xlsx").OrderBy(x=>x).ToArray();
            var fileArray = Directory.GetFiles(path, "*.xlsx", SearchOption.AllDirectories).OrderBy(s => s).FirstOrDefault();
            long counter = 0;
            var file = ReadExcelToFileStream(fileArray);
            var stateData = await ReadExcelModel(file);
            var states = new List<Country>();

            var countries = await _countryRepository.GetAllCountriesAsync();
            foreach (var item in countries)
            {
                var details=stateData.FirstOrDefault(x=>x.Code == item.Code);
                if (details is null) continue;

                item.ContinentCode = details.Code2;
            }

           // await _countryRepository.UpdateAsync(countries);

            var gg = "";
        }


        public async Task<List<StateModel>> ReadExcelModel(Stream fs)
        {
            var stateModels = new List<StateModel>();
            try
            {
                using var package = new ExcelPackage(fs);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    //var referralModel = new ReferralModel();

                    string code2 = workSheet.Cells[rowIterator, 3].Value?.ToString();
                    string name = workSheet.Cells[rowIterator, 1].Value?.ToString();
                    string code = workSheet.Cells[rowIterator, 2].Value?.ToString();

                    if (!string.IsNullOrEmpty(code))
                    {
                        var details=stateModels.FirstOrDefault(x => x.Code == code);
                        if(details is not null)continue;
                        // string[] codes = code.Split('-');
                        stateModels.Add(new StateModel
                        {
                            Name = name,
                            Code2 = code2,
                            Code = code
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("TestService", "ReadExcelModel", ex);
            }

            return stateModels;
        }


        public FileStream ReadExcelToFileStream(string filePath)
        {

            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {

                    if (File.Exists(filePath))
                    {
                        var fileName = File.OpenRead(filePath);
                        return fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                // _logService.LogError("PayoutService", "ReadExcelToFileStream", ex);
            }
            return null;
        }

        public async Task UploadInflowAsync()
        {
            try
            {
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "upload", "183.png");
                // path = Path.Combine(path, "183.png");

                string url = await _fileService.UploadFileAsync(path, ".png");
                if (string.IsNullOrEmpty(url))
                {

                }
            }
            catch (Exception ex)
            {
                _logService.LogError("TransactionService", "UploadInflowAsync", ex);
            }
        }

        //public async Task UploadInflowAsync()
        //{
        //    try
        //    {
        //        List<string> businessIds = GetBusinesses();
        //        if (businessIds.Count == 0) return;

        //        foreach (var r in businessIds)
        //        {
        //            try
        //            {
        //                var appResult = await _signUpRepository.AddIndustryAsync(r);
        //                if (!appResult)
        //                {
        //                    _logService.LogInfo("TransactionService", "UploadInflowAsync:: Unable to upload Reference", $" with response");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                _logService.LogError("TransactionService", "UploadInflowAsync", ex);
        //            }
        //            //continue;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError("TransactionService", "UploadInflowAsync", ex);
        //    }
        //}


        public List<string> GetBusinesses()
        {
            string file = _fileService.GetTemplate("monitoring", Templates.Tm);
            List<string> ids = file.Split(';').ToList();
            return ids;
        }
    }
}
