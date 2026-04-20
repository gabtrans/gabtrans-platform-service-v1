using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Data;

namespace GabTrans.Infrastructure.Utility
{
    public class FileService(ILogService logManager, BlobServiceClient blobServiceClient, IWebHostEnvironment hostingEnvironment, IFileStackClientIntegration fileStackClient) : IFileService
    {
        private readonly ILogService _logService = logManager;
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        private readonly IFileStackClientIntegration _fileStackClient = fileStackClient;

        public byte[] WriteExcelFile(string extension, string fileName, DataTable dt)
        {
            // dll referred NPOI.dll and NPOI.OOXML
            byte[] result = null;
            try
            {
                IWorkbook workbook;

                if (extension == "xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
                else if (extension == "xls")
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    throw new Exception("This format is not supported");
                }

                ISheet sheet1 = workbook.CreateSheet("Sheet 1");

                var headerStyle = workbook.CreateCellStyle(); //Formatting
                var headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerStyle.SetFont(headerFont);

                //make a header row
                IRow row1 = sheet1.CreateRow(0);

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row1.CreateCell(j);

                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(columnName);
                }


                //loops through data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        ICell cell = row.CreateCell(j);
                        String columnName = dt.Columns[j].ToString();
                        cell.SetCellValue(dt.Rows[i][columnName].ToString());
                    }
                }

                using (var ms = new System.IO.MemoryStream())
                {
                    workbook.Write(ms);
                    //FileStream fs = new FileStream(path + @"\" + fileName + ".xlsx", FileMode.Create);
                    //workbook.Write(fs);
                    //fs.Close();
                    workbook.Close();
                    result = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("ReportService", "WritetoExcel", ex);
            }
            return result;
        }

        public byte[] WriteExcelPath(string extension, string path, string fileName, DataTable dt)
        {
            // dll referred NPOI.dll and NPOI.OOXML
            byte[] result = null;
            try
            {
                IWorkbook workbook;

                if (extension == "xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
                else if (extension == "xls")
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    throw new Exception("This format is not supported");
                }

                ISheet sheet1 = workbook.CreateSheet("Sheet 1");

                var headerStyle = workbook.CreateCellStyle(); //Formatting
                var headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerStyle.SetFont(headerFont);

                //make a header row
                IRow row1 = sheet1.CreateRow(0);

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row1.CreateCell(j);

                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(columnName);
                }


                //loops through data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        ICell cell = row.CreateCell(j);
                        String columnName = dt.Columns[j].ToString();
                        cell.SetCellValue(dt.Rows[i][columnName].ToString());
                    }
                }

                using (var ms = new System.IO.MemoryStream())
                {
                    workbook.Write(ms);
                    FileStream fs = new FileStream(path + @"\" + fileName + ".xlsx", FileMode.Create);
                    workbook.Write(fs);
                    fs.Close();
                    workbook.Close();
                    result = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("ReportService", "WritetoExcel", ex);
            }
            return result;
        }

        public byte[] WriteExcelFile()
        {
            throw new NotImplementedException();
        }


        public string ReadExcelToBase64String(string filePath)
        {
            string fileName = "";
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {

                    if (File.Exists(filePath))
                    {
                        Byte[] imageBytes = File.ReadAllBytes(filePath);

                        fileName = Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "ReadExcelToBase64String", ex);
            }
            return fileName;
        }

        public byte[] ReadFileToByte(string filePath)
        {
            byte[] fileName = null;
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {

                    if (File.Exists(filePath))
                    {
                        fileName = File.ReadAllBytes(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "ReadFileToByte", ex);
            }
            return fileName;
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
                _logService.LogError("FileService", "ReadExcelToFileStream", ex);
            }
            return null;
        }


        public DataTable ReadExcelToDataTable(string filePath)
        {
            var fileName = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {

                    if (File.Exists(filePath))
                    {
                        Byte[] imageBytes = File.ReadAllBytes(filePath);

                        // fileName = Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "ReadExcelToDataTable", ex);
            }
            return fileName;
        }



        public bool SaveFile(string filePath, byte[] file)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {

                    if (!File.Exists(filePath))
                    {
                        File.WriteAllBytes(filePath, file);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "SaveFile", ex);
            }
            return false;
        }



        public byte[] GeneratePDF(string template)
        {
            byte[] result = null;
            try
            {
                //string url = _baseService.GetAppSettings("WebUrl") + "/images";
                //template = template.Replace("images", "").Replace("", url);
                //result = OpenHtmlToPdf.Pdf.From(template).Content();
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "GeneratePDF", ex);
            }
            return result;
        }

        public string SaveDocument(byte[] document, long id, string classifier, long doctType)
        {
            string subDirectory = "";
            string fileDirectory = "";

            try
            {

                string baseFolder = "";

                if (document != null)
                {
                    fileDirectory = doctType switch
                    {
                        (long)DocumentTypes.SELFIE => "selfie",
                        (long)DocumentTypes.IDCARD => "idcard",
                        (long)DocumentTypes.UTILITY => "utility",
                        _ => "certificates",
                    };

                    //subDirectory = classifier switch
                    //{
                    //    JumioClassifiers.Front => "front",
                    //    JumioClassifiers.Face => "face",
                    //    JumioClassifiers.FaceMap => "facemap",
                    //    _ => "back",
                    //};

                    string fileName = id + ".png";

                    fileDirectory = Path.Combine(_hostingEnvironment.WebRootPath, baseFolder, fileDirectory, subDirectory);

                    var filePath = Path.Combine(fileDirectory, fileName);

                    if (!Directory.Exists(fileDirectory))
                    {
                        Directory.CreateDirectory(fileDirectory);
                    }

                    if (!File.Exists(filePath))
                    {
                        File.WriteAllBytes(filePath, document);
                    }
                    else
                    {
                        File.Delete(filePath);

                        File.WriteAllBytes(filePath, document);
                    }
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "SaveUserDocument", ex);
            }
            return string.Empty;
        }


        public string SaveDocument(string document, long id, string classifier, long doctType)
        {
            string subDirectory = "";
            string fileDirectory = "";

            try
            {
                string baseFolder = "";

                if (document != null)
                {
                    fileDirectory = doctType switch
                    {
                        (long)DocumentTypes.SELFIE => "selfie",
                        (long)DocumentTypes.IDCARD => "idcard",
                        (long)DocumentTypes.UTILITY => "utility",
                        _ => "certificates",
                    };

                    //subDirectory = classifier switch
                    //{
                    //    JumioClassifiers.Front => "front",
                    //    JumioClassifiers.Face => "face",
                    //    JumioClassifiers.FaceMap => "facemap",
                    //    _ => "back",
                    //};

                    string fileExtension = GetFileExtension(document);

                    string fileName = id + "." + fileExtension;

                    fileDirectory = Path.Combine(_hostingEnvironment.WebRootPath, baseFolder, fileDirectory, subDirectory);

                    var filePath = Path.Combine(fileDirectory, fileName);

                    if (!Directory.Exists(fileDirectory))
                    {
                        Directory.CreateDirectory(fileDirectory);
                    }

                    if (!File.Exists(filePath))
                    {
                        byte[] imageBytes = Convert.FromBase64String(document);
                        File.WriteAllBytes(filePath, imageBytes);
                    }
                    else
                    {
                        File.Delete(filePath);

                        byte[] imageBytes = Convert.FromBase64String(document);
                        File.WriteAllBytes(filePath, imageBytes);
                    }
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "SaveUserDocument", ex);
            }
            return string.Empty;
        }


        public string GetDocumentPath(long id, string classifier, long doctType)
        {
            string filePath = "";
            string subDirectory = "";
            string fileDirectory = "";

            try
            {
                string baseFolder = "";

                string appUrl = "";

                fileDirectory = doctType switch
                {
                    (long)DocumentTypes.SELFIE => "selfie",
                    (long)DocumentTypes.IDCARD => "idcard",
                    _ => "certificates",
                };

                //subDirectory = classifier switch
                //{
                //    JumioClassifiers.Front => "front",
                //    JumioClassifiers.Face => "face",
                //    JumioClassifiers.FaceMap => "facemap",
                //    _ => "back",
                //};

                filePath = id + ".png";

                fileDirectory = Path.Combine(appUrl, baseFolder, fileDirectory, subDirectory);

                filePath = Path.Combine(fileDirectory, filePath);

                if (File.Exists(filePath)) return filePath;
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "GetDocumentPath", ex);
            }
            return filePath;
        }


        public string[] SaveDocuments(string[] documents, long id, long doctType)
        {
            string fileName = "";
            string[] files = new string[] { };
            string document = "";
            string fileDirectory = "";

            try
            {
                string baseFolder = "";

                foreach (var i in documents)
                {
                    if (!string.IsNullOrEmpty(i)) document = i.Substring(i.IndexOf(',') + 1);

                    if (!string.IsNullOrEmpty(document))
                    {
                        fileDirectory = doctType switch
                        {
                            (long)DocumentTypes.SELFIE => "Passport",
                            (long)DocumentTypes.IDCARD => "IdCard",
                            _ => "Certificates",
                        };

                        string fileExtension = GetFileExtension(document);
                        fileName = id + "." + fileExtension;

                        //string directory = Path.Combine(baseFolder,fileDirectory);

                        fileDirectory = Path.Combine(baseFolder, fileDirectory);

                        var filePath = Path.Combine(fileDirectory, fileName);

                        if (!Directory.Exists(fileDirectory))
                        {
                            Directory.CreateDirectory(fileDirectory); //Create directory if it doesn't exist
                        }

                        files.Append(fileName);
                        if (!File.Exists(filePath))
                        {
                            // imgPath = Path.Combine(fileDirectory, fileName);
                            byte[] imageBytes = Convert.FromBase64String(document);
                            File.WriteAllBytes(filePath, imageBytes);
                        }
                        else
                        {
                            File.Delete(filePath);

                            //string imgPath = Path.Combine(fileDirectory, fileName);
                            byte[] imageBytes = Convert.FromBase64String(document);
                            File.WriteAllBytes(filePath, imageBytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "SaveUserDocuments", ex);
            }
            return files;
        }

        public string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }

        public string GetDocument(string document, long id, long doctType)
        {
            string fileName = "";
            string fileDirectory = "";
            try
            {
                string baseFolder = "";

                if (!string.IsNullOrEmpty(document))
                {
                    fileDirectory = doctType switch
                    {
                        (long)DocumentTypes.SELFIE => "Passport",
                        (long)DocumentTypes.IDCARD => "IdCard",
                        _ => "Certificates",
                    };


                    //string directory = Path.Combine(_environment.ContentRootPath, baseFolder);

                    //fileDirectory = Path.Combine(directory, fileDirectory);

                    //var filePath = Path.Combine(fileDirectory, document);

                    fileDirectory = Path.Combine(baseFolder, fileDirectory);

                    var filePath = Path.Combine(fileDirectory, fileName);

                    filePath = Path.Combine(filePath, document);

                    if (File.Exists(filePath))
                    {
                        // string imgPath = Path.Combine(fileDirectory, document);
                        Byte[] imageBytes = File.ReadAllBytes(filePath);

                        fileName = Convert.ToBase64String(imageBytes);
                        fileName = fileName.Substring(fileName.IndexOf(',') + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "GetUserDocument", ex);
            }
            return fileName;
        }

        public string GetFileURL(string document, long id, long doctType)
        {
            string fileName = "";
            string fileDirectory = "";
            try
            {
                string baseFolder = "";

                if (!string.IsNullOrEmpty(document))
                {
                    fileDirectory = doctType switch
                    {
                        (long)DocumentTypes.SELFIE => "Passport",
                        (long)DocumentTypes.IDCARD => "IdCard",
                        _ => "Certificates",
                    };

                    fileName = baseFolder + "/" + fileDirectory + "/" + document;

                    // var filePath = Path.Combine(fileDirectory, fileName);

                    //fileName = Path.Combine(filePath, id.ToString());
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "GetFileURL", ex);
            }
            return fileName;
        }

        public string GetTemplate(string templateId, long type)
        {
            string file = "";
            string path = "";
            try
            {
                templateId = templateId.Trim();
                switch (type)
                {
                    case Templates.Email:
                        path = Path.Combine(_hostingEnvironment.WebRootPath, "templates", "email");
                        path = Path.Combine(path, templateId + ".html");
                        break;
                    case Templates.Sms:
                        path = Path.Combine(_hostingEnvironment.WebRootPath, "templates", "sms");
                        path = Path.Combine(path, templateId + ".txt");
                        break;
                    case Templates.Receipt:
                        path = Path.Combine(_hostingEnvironment.WebRootPath, "templates", "receipt");
                        path = Path.Combine(path, templateId + ".txt");
                        break;
                    default:
                        path = Path.Combine(_hostingEnvironment.WebRootPath, "templates", "monitoring");
                        path = Path.Combine(path, templateId + ".txt");
                        file = File.ReadAllText(path);
                        break;
                }

                if (!string.IsNullOrEmpty(path)) file = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "GetTemplate", ex);
            }

            return file;
        }

        public string SaveDirectorDocument(byte[] document, string id, string classifier, long doctType)
        {
            string subDirectory = "";
            string fileDirectory = "";

            try
            {
                string baseFolder = "";

                if (document != null)
                {
                    fileDirectory = doctType switch
                    {
                        (long)DocumentTypes.SELFIE => "selfie",
                        (long)DocumentTypes.IDCARD => "idcard",
                        (long)DocumentTypes.UTILITY => "utility",
                        _ => "certificates",
                    };

                    //subDirectory = classifier switch
                    //{
                    //    JumioClassifiers.Front => "front",
                    //    JumioClassifiers.Face => "face",
                    //    JumioClassifiers.FaceMap => "facemap",
                    //    _ => "back",
                    //};

                    string fileName = id + ".png";

                    fileDirectory = Path.Combine(_hostingEnvironment.WebRootPath, baseFolder, fileDirectory, subDirectory);

                    var filePath = Path.Combine(fileDirectory, fileName);

                    if (!Directory.Exists(fileDirectory))
                    {
                        Directory.CreateDirectory(fileDirectory);
                    }

                    if (!File.Exists(filePath))
                    {
                        File.WriteAllBytes(filePath, document);
                    }
                    else
                    {
                        File.Delete(filePath);

                        File.WriteAllBytes(filePath, document);
                    }
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "SaveDirectorDocument", ex);
            }
            return string.Empty;
        }


        public byte[] ConvertToPdf(string htmlContent)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    //page.Margin(2, Unit.Centimetre);
                    //page.Background(Colors.White);
                    //page.DefaultTextStyle(x => x.FontSize(20));

                    page.Content()
                        .Text(htmlContent);
                    //.SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    //page.Content()
                    //    .PaddingVertical(1, Unit.Centimetre)
                    //    .Column(x =>
                    //    {
                    //        x.Spacing(20);

                    //        x.Item().Text(Placeholders.LoremIpsum());
                    //        x.Item().Image(Placeholders.Image(200, 100));
                    //    });

                    //page.Footer()
                    //    .AlignCenter()
                    //    .Text(x =>
                    //    {
                    //        x.Span("Page ");
                    //        x.CurrentPageNumber();
                    //    });
                });
            }).GeneratePdf(); //.GeneratePdf("hello.pdf");
        }

        public async Task<string> UploadFileAsync(string base64File)
        {
            string fileExtension = GetFileExtension(base64File);

            string derivFileExtension = $"image/{fileExtension}";

            byte[] imageBytes = Convert.FromBase64String(base64File);

            var fileStackResponse = await _fileStackClient.Upload(imageBytes, derivFileExtension);

            if (fileStackResponse is null || fileStackResponse.url is null) return string.Empty;

            return fileStackResponse.url.ToString();
        }

        public async Task<string> UploadFileAsync(byte[] byteArrayFile)
        {
            string base64File = Convert.ToBase64String(byteArrayFile);

            string fileExtension = GetFileExtension(base64File);

            string derivFileExtension = $"image/{fileExtension}";

            var fileStackResponse = await _fileStackClient.Upload(byteArrayFile, derivFileExtension);

            if (fileStackResponse is null || fileStackResponse.url is null) return string.Empty;

            return fileStackResponse.url.ToString();
        }

        public async Task<string> UploadFileAsync(string filePath, string fileExtension)
        {
            try
            {
                var file = ReadFileToFileStream(filePath);
                if (file == null) return string.Empty;

                var containerClient = _blobServiceClient.GetBlobContainerClient("gabtrans-files");
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob); // Create if not present

                var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + fileExtension); // Generate a unique name

                await blobClient.UploadAsync(file, true);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "UploadFileAsync", ex);
            }

            return string.Empty;
        }

        public async Task<FileObject> DownloadFileAsync(string fileName)
        {
            try
            {
                //var file = ReadFileToFileStream(filePath);
                //if (file == null) return string.Empty;

                //fileName = "https://gabtrans.blob.core.windows.net/gabtrans-files/d815f83a-f6ef-4882-a9d4-8a7400cd710d.png";
                //string[] files = fileName.Split('/');

                //fileName = files[4];

                var containerClient = _blobServiceClient.GetBlobContainerClient("gabtrans-files");

                var blobClient = containerClient.GetBlobClient(fileName);
                if (!await blobClient.ExistsAsync())
                {
                    return null; // File not found
                }

                // string path = $"C:Users\\MY SPECTRE\\Desktop\\Test\\Test\\{fileName}";
                // await blobClient.DownloadToAsync(path);

                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0;
                var properties = await blobClient.GetPropertiesAsync();
                var contentType = properties.Value.ContentType.Split('/')[1];
                return new FileObject { Stream = stream, Extension = contentType, FileName = fileName };
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "UploadFileAsync", ex);
            }

            return new FileObject();
        }

        public string GetFileName(string fileName)
        {
            try
            {
                string[] files = fileName.Split('/');

                return files[^1];
            }
            catch (Exception ex)
            {
                _logService.LogError("FileService", "GetFileName", ex);
            }

            return string.Empty;
        }

        public FileStream ReadFileToFileStream(string filePath)
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
                _logService.LogError("FileService", "ReadExcelToFileStream", ex);
            }
            return null;
        }
    }
}
