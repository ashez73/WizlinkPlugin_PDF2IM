using System;
using System.ComponentModel;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using System.Drawing.Imaging;

namespace WizlinkPlugin_PDF2IM
{
    [WizlinkVisible]
    [Description("Wizlink Plugin Template")]
    public class Plugin : PluginBase
    {
        [Description("Method converts .pdf. to image format")]
        [return: TupleDescription(new string[] { "Status", "Date" })]
        public (string myStatus, string date) ConvertIt([Description("Convert your .pdf")] string filePath, string fileType, int dpi, int page)
        {
            base.Log($"Log from plugin: {filePath} conversion is being started");
            PdfDocument pdf = new PdfDocument();
            pdf.LoadFromFile(filePath);
            Image image = pdf.SaveAsImage(page - 1, PdfImageType.Bitmap, dpi, dpi);
            ImageFormat newFormat = GetImageFormat(fileType);
            string myStatus;
            if (newFormat != null)
            {
                string newFilePath = $"{filePath.Remove(filePath.Length - 4, 3)}_{page}.{fileType}";
                try
                {
                    image.Save(newFilePath, newFormat);
                    myStatus = "OK";
                    base.Log($"Log from plugin: conversion successful");
                }
                catch (Exception ex)
                {
                    myStatus = $"Error: {ex.Message}";
                }
            }
            else
            {
                myStatus = "Unsupported file type";
                base.Log($"Unsupported file type: {fileType}");
            }
            var dateTime = DateTimeOffset.Now;
            return (myStatus, dateTime.ToString());
        }

        private ImageFormat GetImageFormat(string fileType)
        {
            switch (fileType.ToLower())
            {
                case "bmp":
                    return ImageFormat.Bmp;
                case "gif":
                    return ImageFormat.Gif;
                case "jpeg":
                case "jpg":
                    return ImageFormat.Jpeg;
                case "png":
                    return ImageFormat.Png;
                default:
                    return null;
            }
        }

    }
}