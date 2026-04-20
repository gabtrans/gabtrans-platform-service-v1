using GabTrans.Domain.Models;
using System.Data;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IFileService
    {
        FileStream ReadFileToFileStream(string filePath);
        byte[] ConvertToPdf(string htmlContent);
        byte[] GeneratePDF(string template);
        string ReadExcelToBase64String(string filePath);
        byte[] ReadFileToByte(string filePath);
        FileStream ReadExcelToFileStream(string filePath);
        DataTable ReadExcelToDataTable(string filePath);
        string GetTemplate(string templateId, long type);
        bool SaveFile(string filePath, byte[] file);
        byte[] WriteExcelFile();
        string GetDocumentPath(long id, string classifier, long doctType);
        string SaveDocument(byte[] document, long id, string classifier, long doctType);
        string SaveDocument(string document, long id, string classifier, long doctType);
        string[] SaveDocuments(string[] documents, long id, long doctType);
        string GetDocument(string document, long id, long doctType);
        string GetFileURL(string document, long id, long doctType);
        byte[] WriteExcelPath(string extension, string path, string fileName, DataTable dt);
        string SaveDirectorDocument(byte[] document, string id, string classifier, long doctType);
        Task<string> UploadFileAsync(string base64File);
        Task<string> UploadFileAsync(byte[] byteArrayFile);
        Task<string> UploadFileAsync(string filePath, string fileExtension);
        Task<FileObject> DownloadFileAsync(string fileName);
        string GetFileName(string fileName);
    }
}
