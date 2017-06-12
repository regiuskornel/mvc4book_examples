using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Controllers.Files;

namespace MvcApplication1
{

    //A 2. példa nem használja
    public class ExtendedMetaDataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes,
            Type containerType, Func<object> modelAccessor,
            Type modelType, string propertyName)
        {
            var attributeslist = attributes.ToList();

            var metadata = base.CreateMetadata(attributeslist, containerType, modelAccessor, modelType, propertyName);
            var fileuploadAttr = attributeslist.OfType<FileUploadValidationAttribute>().FirstOrDefault();
            if (fileuploadAttr != null)
                metadata.AdditionalValues.Add(FileUploadValidationAttribute.FileUploadValidationName, fileuploadAttr);
            return metadata;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FileUploadValidationAttribute : ValidationAttribute, IMetadataAware
    {
        public const string FileUploadValidationName = "FileUploadVA";

        private readonly string[] _usableFileTypes;
        private readonly Int64 _maxlength;
        private bool _isMultiple;

        public FileUploadValidationAttribute(Int64 MaxLengthKB = 8192, bool Multiple = true, string FileTypes = "image/png,image/gif,image/jpg")
        {
            this._maxlength = MaxLengthKB * 1024;
            this._usableFileTypes = FileTypes.Split(new char[] { ',', ';' });
            this._isMultiple = Multiple;
        }

        public string UsableFileTypes
        {
            get { return string.Join(",", _usableFileTypes); }
        }

        public bool Multiple
        {
            get { return _isMultiple; }
        }

        protected override ValidationResult IsValid(object filesToValidate, ValidationContext validationContext)
        {
            var filemodel = validationContext.ObjectInstance as FileModel;
            if (filemodel != null)
            {
                var files = filesToValidate as IEnumerable<HttpPostedFileBase>;
                if (files == null)
                {
                    //Valid, mert már fel lett töltve?
                    var dbFileModel = FileModel.GetById(filemodel.Id);
                    if (dbFileModel.Status == FileModel.UploadStatus.Temp)
                        return ValidationResult.Success;

                    return NoFiles();
                }

                foreach (var postedFile in files)
                {
                    if (postedFile == null) return NoFiles();

                    if (postedFile.ContentLength > _maxlength)
                    {
                        return new ValidationResult(string.Format("A {0} fájl nagyobb, mint {1}KB!", postedFile.FileName, _maxlength / 1024));
                    }
                    //A kliensben nem bízunk.
                    if (!UsableFileTypes.Contains(postedFile.ContentType))
                    {
                        return new ValidationResult("Ilyen fájltípus nem tölthető fel!");
                    }
                }
            }
            return ValidationResult.Success;
        }

        private ValidationResult NoFiles()
        {
            return new ValidationResult("Jelölj ki fájlt a feltöltéshez!");
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues.Add(FileUploadValidationName, this);
        }
    }
}