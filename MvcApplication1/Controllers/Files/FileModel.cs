using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication1.Controllers.Files
{
public class FileModel
{
    //A modell állapota
    public enum UploadStatus
    {
        None,
        Temp,
        Uploaded,
        Deleting
    }

    public Guid Id { get; set; }

    public string UserId { get; set; }

    [Display(Name = "Fájlnév")]
    public string FileName { get; set; }

    [Display(Name = "Leírás")]
    [Required(ErrorMessage = "A leírást meg kell adni")]
    public string Description { get; set; }

    [FileUploadValidation(100, true, "image/png")]
    public List<HttpPostedFileBase> Files { get; set; }

    [Display(Name = "Fájlméret")]
    public long Length { get; set; }

    public string Path { get; set; }

    public UploadStatus Status { get; set; }

    [Display(Name = "MIME típus")]
    public string MIME { get; set; }


    #region InMemory persisztencia
    public static IList<FileModel> GetList()
    {
        return datalist ?? (datalist = new List<FileModel>());
    }

    public static FileModel GetById(string id)
    {
        Guid gid;
        if (datalist == null || !Guid.TryParse(id, out gid))
            return null;
        return datalist.FirstOrDefault(f => f.Id == gid);
    }

    public static FileModel GetById(Guid id)
    {
        return datalist.FirstOrDefault(f => f.Id == id);
    }

    public static void Add(FileModel fileModel)
    {
        if (datalist == null) datalist = new List<FileModel>();
        datalist.Add(fileModel);
    }

    public static void AddRange(IEnumerable<FileModel> fileModel)
    {
        if (datalist == null) datalist = new List<FileModel>();
        datalist.AddRange(fileModel);
    }

    private static List<FileModel> datalist;
    #endregion
}
}