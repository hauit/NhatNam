using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsPartData
    {
        public int id = 0;
        public DateTime inputDate = DateTime.Now;
        public string partID          = string.Empty;
        public string materialID      = string.Empty;
        public string materialType    = string.Empty;
        public string workpiecesize   = string.Empty;
        public string shape           = string.Empty;
        public float thickness       = 0;
        public string width           = string.Empty;
        public float lenght          = 0;
        public bool cut             = false;
        public bool rawMachine      = false;
        public bool handFinish      = false;
        public bool hairLine        = false;
        public bool wAnod           = false;
        public bool bAnod           = false;
        public bool blast30         = false;
        public bool blast60         = false;
        public bool seal            = false;
        public bool migaki          = false;
        public bool bafu            = false;
        public bool cleanwave       = false;
        public bool vacPac          = false;
        public bool helisert        = false;
        public bool serialNo        = false;
        public bool palCoat         = false;
        public bool paint           = false;
        public bool bBD             = false;
        public string otherpro        = string.Empty;
        public decimal price           = 0;
        public string memo            = string.Empty;
        public string note            = string.Empty;
        public bool caciras         = false;
        public bool inside          = false;
        public bool maBong          = false;
        public bool inLuoi          = false;
        public bool heru            = false;
        public bool niken           = false;
        public bool maiBongDP = false;
        protected bool deleted = false;

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
        public int ID { get { return id; } set { id = value; } }
        public DateTime InputDate { get { return inputDate; } set { inputDate = value; } }
        public string PartID { get { return partID; } set { partID = value; } }
        public string MaterialID { get { return materialID; } set { materialID = value; } }
        public string MaterialType { get { return materialType; } set { materialType = value; } }
        public string Workpiecesize { get { return workpiecesize; } set { workpiecesize = value; } }
        public string Shape { get { return shape; } set { shape = value; } }
        public float Thickness { get { return thickness; } set { thickness = value; } }
        public string Width { get { return width; } set { width = value; } }
        public float Lenght { get { return lenght; } set { lenght = value; } }
        public bool Cut { get { return cut; } set { cut = value; } }
        public bool RawMachine { get { return rawMachine; } set { rawMachine = value; } }
        public bool HandFinish { get { return handFinish; } set { handFinish = value; } }
        public bool HairLine { get { return hairLine; } set { hairLine = value; } }
        public bool WAnod { get { return wAnod; } set { wAnod = value; } }
        public bool BAnod { get { return bAnod; } set { bAnod = value; } }
        public bool Blast30 { get { return blast30; } set { blast30 = value; } }
        public bool Blast60 { get { return blast60; } set { blast60 = value; } }
        public bool Seal { get { return seal; } set { seal = value; } }
        public bool Migaki { get { return migaki; } set { migaki = value; } }
        public bool Bafu { get { return bafu; } set { bafu = value; } }
        public bool Cleanwave { get { return cleanwave; } set { cleanwave = value; } }
        public bool VacPac { get { return vacPac; } set { vacPac = value; } }
        public bool Helisert { get { return helisert; } set { helisert = value; } }
        public bool SerialNo { get { return serialNo; } set { serialNo = value; } }
        public bool PalCoat { get { return palCoat; } set { palCoat = value; } }
        public bool Paint { get { return paint; } set { paint = value; } }
        public bool BBD { get { return bBD; } set { bBD = value; } }
        public string Otherpro { get { return otherpro; } set { otherpro = value; } }
        public decimal Price { get { return price; } set { price = value; } }
        public string Memo { get { return memo; } set { memo = value; } }
        public string Note { get { return note; } set { note = value; } }
        public bool Caciras { get { return caciras; } set { caciras = value; } }
        public bool Inside { get { return inside; } set { inside = value; } }
        public bool MaBong { get { return maBong; } set { maBong = value; } }
        public bool InLuoi { get { return inLuoi; } set { inLuoi = value; } }
        public bool Heru { get { return heru; } set { heru = value; } }
        public bool Niken { get { return niken; } set { niken = value; } }
        public bool MaiBongDP { get { return maiBongDP; } set { maiBongDP = value; } }
    }
}
