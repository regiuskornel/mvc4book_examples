using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MvcApplication1.Controllers.Files;

namespace MvcApplication1.Controllers
{
    public class FileAccessController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase uploadedfile)
        {
            if (uploadedfile != null && uploadedfile.ContentLength > 0)
            {
                if (uploadedfile.ContentType != "image/png")
                    return new ContentResult() { Content = "Csak PNG fájlt tölthetsz fel!" };

                var fileName = Path.GetFileName(uploadedfile.FileName);
                var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                uploadedfile.SaveAs(path);
            }
            return RedirectToAction("Index");
        }

        public ActionResult UploadMulti()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadMulti(HttpPostedFileBase[] uploadedfile)
        {
            foreach (var file in uploadedfile)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    file.SaveAs(path);
                }
            }
            return RedirectToAction("Index");
        }


        /*kétlépéses upload*/
public ActionResult UploadList()
{
    var newfiles = FileModel.GetList().Where(f => f.UserId == Session.SessionID && f.Status == FileModel.UploadStatus.Uploaded);
    return View(newfiles);
}


public ActionResult UploadNew()
{
    return View();
}

[HttpPost]
public ActionResult UploadNew(FileModel model)
{
    //Csak a 'Files' érdekes most.
    if (!ModelState.IsValidField("Files"))
    {
        return View(model);
    }

    var newfiles = new List<FileModel>();
    foreach (var file in model.Files)
    {
        if (file != null && file.ContentLength > 0) //A validációs attribútum miatt felesleges.
        {
            var newfile = new FileModel
            {
                Id = Guid.NewGuid(),
                UserId = Session.SessionID, //sessionid as userid -> test only
                Status = FileModel.UploadStatus.Temp,
                FileName = file.FileName,
                Length = file.ContentLength,
                MIME = file.ContentType
            };
            var fileName = newfile.Id.ToString();
            var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
            newfile.Path = path;
            file.SaveAs(path);
            newfiles.Add(newfile);
        }
    }
    if (newfiles.Count > 0)
    {
        FileModel.AddRange(newfiles);
        return RedirectToAction("UploadFill");
    }
    return RedirectToAction("UploadList");
}

public ActionResult UploadFill()
{
    var newfiles = FileModel.GetList().Where(f => f.UserId == Session.SessionID && f.Status == FileModel.UploadStatus.Temp);
    return View(newfiles);
}

[HttpPost]
public ActionResult UploadFill(List<FileModel> postedfileslist)
{
    var newfiles = FileModel.GetList().Where(f => f.UserId == Session.SessionID && f.Status == FileModel.UploadStatus.Temp).ToList();
    var remaininvalid = new List<FileModel>();

    for (int i = 0; i < postedfileslist.Count; i++)
    {
        var posted = postedfileslist[i];
        var fileModel = newfiles.FirstOrDefault(f => f.Id == posted.Id);
        if (fileModel == null)
        {
            posted.Status = FileModel.UploadStatus.Deleting;
            continue;
        }
        var descriptionkey = string.Format("[{0}].Description", i);
        var descmodelstate = ModelState[descriptionkey];
        if (descmodelstate.Errors.Count == 0)
        {
            fileModel.Description = posted.Description;
            fileModel.Status = FileModel.UploadStatus.Uploaded;
            //UpdateToDataBase(fileModel);
        }
        else
            remaininvalid.Add(fileModel);

    }
    if (remaininvalid.Count == 0)
        return RedirectToAction("UploadList");

    //1. Miért van erre szükség?
    ModelState.Clear();
    for (int i = 0; i < remaininvalid.Count; i++)
    {
        var descriptionkey = string.Format("[{0}].Description", i);
        var ms = new ModelState();
        ms.Errors.Add("A leírást meg kell adni");
        ModelState.Add(descriptionkey, ms);
    }

    return View(remaininvalid);
}

public ActionResult DownloadFile(string id)
{
    if (!string.IsNullOrWhiteSpace(id))
    {
        var model = FileModel.GetById(id);
        if (model != null)
        {
            if (System.IO.File.Exists(model.Path))
            {
                return new FilePathResult(model.Path, model.MIME)
                {
                    FileDownloadName = model.FileName
                };
            }
        }
    }
    return RedirectToAction("UploadList");
}
    }


}
