using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using BassinoBase.Helpers;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class ProductController : AbmController<Product, ProductViewModel, ProductViewModel>
    {
        private readonly IMeasureUnitDao _measureUnitDao;
        private readonly IProviderDao _providers;
        private readonly IProductDao _productDao;
        OperationResult operationResult = new OperationResult();
        private readonly IProductViewModelMapper _productViewModelMapper;
        private readonly ICreditoDao _creditoDao;

        public ProductController(IProductViewModelMapper productViewModelMapper, IProductDao productDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IMeasureUnitDao measureUnitDao, IProviderDao providers, IProductDao productDao1, IProductViewModelMapper productViewModelMapper1, ICreditoDao creditoDao)
			: base(abmControllerBahavior, productDao, productViewModelMapper, unitOfWorkHelper)
        {
            _measureUnitDao = measureUnitDao;
            _providers = providers;
            _productDao = productDao1;
            _productViewModelMapper = productViewModelMapper1;
            _creditoDao = creditoDao;
        }


        public override ActionResult Details(int id)
        {
            ViewBag.imagenes = _productDao.Get(id).Imagenes;
            ViewBag.imagenesAny = _productDao.Get(id).Imagenes.Any();
            return base.Details(id);
        }

        public override void BeforeDelete(Product model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _productDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null && model.Secuencia!=null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Product model, ProductViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
                model.Secuencia = GetLastSecuence();
                model.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateCreate = DateTime.Now;
            }
            else
            {
                model.UpdateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateUpdate = DateTime.Now;

            }
            base.BeforeSave(model, viewModel, isNew);
        }
        public JsonResult GetProductCold(string text)
        {
            var cargas = _productDao.Get(Convert.ToInt32(text));
            if (cargas != null)
            {
                return Json(
 (new
 {
    cold=cargas.IsCold
 }), JsonRequestBehavior.AllowGet);
            }


            return Json(cargas, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetDataPrduct(string text)
        {
            var cargas = _productDao.Get(Convert.ToInt32(text));
            
            var promo = "";
            if (cargas.Promociones.Any())
            {
                var promocion = cargas.Promociones.FirstOrDefault(d => d.Activada);
                promo = promocion.Porcentage.ToString();
            }

            if (cargas != null)
            {
                return Json(
 (new
 {
     promo=promo,
     monto = cargas.Precio
 }), JsonRequestBehavior.AllowGet);
            }


            return Json(cargas, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetDataPrducts(string text)
        {
            var vector = text.Split('-');
            decimal precio = 0;
            foreach (var item in vector)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    var cargas = _productDao.Get(Convert.ToInt32(item));
                    precio=precio+cargas.Precio;
                }
            }
            
                return Json(
 (new
 {
     promo = precio.ToString().Replace('.',','),
     monto = precio.ToString().Replace('.', ',')
 }), JsonRequestBehavior.AllowGet);
           
        }
        public JsonResult GetDataPrductsCredito(string text, int idCredito)
        {
            var vector = text.Split('-');
            var credito = _creditoDao.Get(idCredito);
           
            decimal precio = 0;
            decimal precioSub = 0;
            foreach (var item in vector)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    var cargas = _productDao.Get(Convert.ToInt32(item));
                    precio=precio+cargas.Precio;
                }
            }
            foreach (var item in vector)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    var cargas = _productDao.Get(Convert.ToInt32(item));
                    if(!credito.Producto.Contains(cargas))
                    precioSub = precioSub + cargas.Precio;
                }
            }
            var montoCredito = credito.Monto + precioSub; 
                return Json(
 (new
 {
     promo = precio.ToString().Replace('.',','),
     monto = precio.ToString().Replace('.', ','),
     montoTotal=montoCredito.ToString().Replace('.',',')
 }), JsonRequestBehavior.AllowGet);
           
        }
        public ActionResult GetAutoComplete(string texto)
        {
            var result = _productDao.GetAutoComplete(texto).Take(10);

            var viewModels = _productViewModelMapper.Map(result);

            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteImg(string id, string productoId)
        {
            var cargas = _productDao.Get(Convert.ToInt32(productoId));
            bool bandera = true;
            if (cargas != null)
            {
                var image = cargas.Imagenes.FirstOrDefault(r => r.Id == Convert.ToInt32(id));
                if (image != null)
                    cargas.Imagenes.Remove(image);
                try
                {
                    _productDao.Save();

                }
                catch (Exception)
                {
                    bandera = false;
                    
                }
            }


            return Json(
   (new
   {
       resultado = bandera
   }), JsonRequestBehavior.AllowGet);

        }

        public override ActionResult GridInfo(DataSourceRequest request, string filtro)
        {
            var result = DAO.GetAllQ(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddImages(int id)
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.productoId = id;
                return PartialView("AddImages");
            }
            else
            {
                return new HttpNotFoundResult();
            }
           
            return PartialView();
        }
        public ActionResult DeleteImages(int id)
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.Imagenes = _productDao.Get(id).Imagenes;
                ViewBag.productoId = id;
                return PartialView("DeleteImages");
            }
            else
            {
                return new HttpNotFoundResult();
            }
           
            return PartialView();
        }
        [HttpPost]
        public JsonResult UplodMultiple(HttpPostedFileBase[] uploadedFiles,string name)
        {
            string _imgname = string.Empty;
            List<Attachment> newAttachmentList = new List<Attachment>();
            foreach (var File in uploadedFiles)
            {
                if (File != null && File.ContentLength > 0)
                {
                    byte[] FileByteArray = new byte[File.ContentLength];
                    File.InputStream.Read(FileByteArray, 0, File.ContentLength);
                    Imagenes newAttchment = new Imagenes();
                    //newAttchment.FileName = File.FileName;
                    newAttchment.FileType = File.ContentType;
                    newAttchment.FileContent = FileByteArray;
                    _imgname = Guid.NewGuid().ToString();
                    var _ext = Path.GetExtension(File.FileName);
                    var _comPath = Server.MapPath("/Content/Images/") + _imgname + _ext;
                    var _com200Path = Server.MapPath("/Content/Images/200x200") + _imgname + _ext;
                    var _com400Path = Server.MapPath("/Content/Images/400x400") + _imgname + _ext;
                    newAttchment.Path = _comPath;
                    newAttchment.FileName = _imgname + _ext;
                    _imgname = "MVC_" + _imgname + _ext;

                    ViewBag.Msg = _comPath;
                    var path = _comPath;
                    File.SaveAs(path);
                    MemoryStream ms = new MemoryStream();
                    WebImage img = new WebImage(_comPath);

                    if (img.Width > 200)
                        img.Resize(200, 200,true);
                    img.Save(_com200Path);
                    WebImage img2 = new WebImage(_comPath);
                    if (img2.Width > 400)
                        img2.Resize(400, 400,true);
                    img2.Save(_com400Path);

                    var model = _productDao.Get(Convert.ToInt32(name));
                    if (model.Imagenes == null)
                        model.Imagenes = new List<Imagenes>();

                    model.Imagenes.Add(newAttchment);
                }
            }
            try
            {
                _productDao.Save();
                operationResult.Success = true;
                operationResult.Message = "Attachment Added Successfully";
            }
            catch (Exception)
            {
                operationResult.Success = false;
                operationResult.Message = "An Error Ocured During saving the new Attachment ";

            }
            if (operationResult.Success)
            {
                string HTMLString = "";// CaptureHelper.RenderViewToString("_AttachmentBulk", newAttachmentList, this.ControllerContext);
                return Json(new
                {
                    statusCode = 200,
                    status = operationResult.Message,
                    NewRow = HTMLString
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    statusCode = 400,
                    status = operationResult.Message
                }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult AddImages(HttpPostedFileBase uploadedFile)
        {
            string name = Request.Params["data"];
            if (uploadedFile != null && uploadedFile.ContentLength > 0)
            {
                byte[] FileByteArray = new byte[uploadedFile.ContentLength];
                uploadedFile.InputStream.Read(FileByteArray, 0, uploadedFile.ContentLength);
                Imagenes newAttchment = new Imagenes();
                newAttchment.FileName = uploadedFile.FileName;
                newAttchment.FileType = uploadedFile.ContentType;
                newAttchment.FileContent = FileByteArray;
                var model = _productDao.Get(Convert.ToInt32(name));
                if (model.Imagenes == null)
                    model.Imagenes = new List<Imagenes>();

                model.Imagenes.Add(newAttchment);
                try
                {
                    _productDao.Save();
                    operationResult.Success = true;
                    operationResult.Message = "Attachment Added Successfully";
                }
                catch (Exception)
                {
                    operationResult.Success = false;
                    operationResult.Message = "An Error Ocured During saving the new Attachment ";
                    
                }
                

               
                if (operationResult.Success)
                {
                    string HTMLString = CaptureHelper.RenderViewToString("_AttachmentItem", newAttchment, this.ControllerContext);
                    return Json(new
                    {
                        statusCode = 200,
                        status = operationResult.Message,
                        NewRow = HTMLString
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        statusCode = 400,
                        status = operationResult.Message,
                        file = uploadedFile.FileName
                    }, JsonRequestBehavior.AllowGet);

                }
            }
            return Json(new
            {
                statusCode = 400,
                status = "Bad Request! Upload Failed",
                file = string.Empty
            }, JsonRequestBehavior.AllowGet);
        }


        public override ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "Productos");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.imprimir = module.Actions.Any(s => s.Description == "Imprimir");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.imprimir = false;
                }
            }
            ViewBag.Title = "Producto";
            return PartialView();
        }
        public override ActionResult Create()
        {
            ViewBag.MeasuresUnits = _measureUnitDao.GetAll();
            ViewBag.Providers = _providers.GetAll();
            return base.Create();
        }

        public override ActionResult Create(ProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MeasuresUnits = _measureUnitDao.GetAll();
                ViewBag.Providers = _providers.GetAll();
                return PartialView(viewModel);
            }
            return base.Create(viewModel);
        }

        public override ActionResult Edit(int id)
        {
            ViewBag.imagenes = _productDao.Get(id).Imagenes;
            ViewBag.imagenesAny = _productDao.Get(id).Imagenes.Any();
            ViewBag.MeasuresUnits = _measureUnitDao.GetAll();
            ViewBag.Providers = _providers.GetAll();
            return base.Edit(id);
        }

        public override ActionResult Edit(ProductViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.imagenes = _productDao.Get(viewModel.Id).Imagenes;
                ViewBag.imagenesAny = _productDao.Get(viewModel.Id).Imagenes.Any();
                ViewBag.MeasuresUnits = _measureUnitDao.GetAll();
                ViewBag.Providers = _providers.GetAll();
                return PartialView(viewModel);
            }
            return base.Edit(viewModel);
        }

        public JsonResult GetProducts()
        {
            var cargas = _productDao.GetAll().Where(d => !d.IsDelete);
          return Json(
    cargas.Select(x => new
    {
        id = x.Id,
        name = x.Nombre +" - "+x.Id.ToString()
    }), JsonRequestBehavior.AllowGet);
        }

    }
    public class OperationResult
    {
        public bool Success;
        public string Message;
        public OperationResult()
        {
            Success = true;
            Message = string.Empty;
        }
        public OperationResult(bool Success, string Message)
        {
            this.Success = Success;
            this.Message = Message;
        }
    }
}