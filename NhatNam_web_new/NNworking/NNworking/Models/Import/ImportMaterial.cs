using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NNworking.Models.Import
{
    public abstract class ImportMaterial : IImport
    {
        protected virtual void CheckOrderExist(string data)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var existed = db.View_242_BusOder.Where(x => x.BOderNo.ToLower() == data.ToLower()).Any();
            if(!existed)
            {
                throw new ArgumentException($@"Order {data} không tồn tại trong Busorder. Vui lòng kiểm tra!");
            }
        }

        protected virtual void CheckSupplierExit(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var existed = db.View_222_Partner.Where(x => x.Code.ToLower() == data.ToLower()).Any();
            if(!existed)
            {
                throw new ArgumentException($@"Nhà cung cấp {data} không tồn tại trong danh sách. Vui lòng kiểm tra!");
            }
        }

        protected virtual void CheckMaterialExist(string data,string supplier)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var existed = db.C222_MaterialStock.Where(x => x.MaterialID.ToLower() == data.ToLower() && x.Supplier.ToLower() == supplier.ToLower()).Any();
            if(!existed)
            {
                var obj = new C222_MaterialStock();
                obj.MaterialID = data;
                obj.Supplier = supplier;
                obj.Note = $@"Auto input in {DateTime.Now.Date.ToString("yyMMdd")}";
                db.C222_MaterialStock.Add(obj);
                db.SaveChanges();
                //throw new ArgumentException($@"Vật liệu {data} không tồn tại trong danh sách vật liệu. Vui lòng kiểm tra!");
            }
        }

        protected virtual void CheckMaterialConfigurationExist(string data, string material)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var existed = db.C222_MaterialConfiguration.Where(x => x.MaterialConfiguration.ToLower() == data.ToLower() && x.MaterialID.ToLower() == material.ToLower()).Any();
            if(!existed)
            {
                var obj = new C222_MaterialConfiguration();
                obj.MaterialConfiguration = data;
                obj.MaterialID = material;
                obj.Note = $@"Auto input in {DateTime.Now.Date.ToString("yyMMdd")}";
                db.C222_MaterialConfiguration.Add(obj);
                db.SaveChanges();
                //throw new ArgumentException($@"Cấu hình {data} không tồn tại trong danh sách cấu hình của vật liệu {material}. Vui lòng kiểm tra!");
            }
        }

        protected virtual void CheckBPYC(string data)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var existed = db.View_222_Partner.Where(x => x.Code.ToLower() == data.ToLower()).Any();
            if(!existed)
            {
                throw new ArgumentException($@"Bộ phận {data} không tồn tại trong danh sách bộ phận. Vui lòng kiểm tra!");
            }
        }
    }
}