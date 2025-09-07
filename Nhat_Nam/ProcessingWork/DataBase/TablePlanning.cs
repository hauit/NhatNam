using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class TablePlanning
    {
        private int iD;
        private string order = string.Empty;
        private string nC = string.Empty;
        private string machineID = string.Empty;
        private string mayGC = string.Empty;
        private int slg = 0;
        private DateTime start = DateTime.Now.AddYears(-80);
        private DateTime finish = DateTime.Now.AddYears(-80);
        private float tGGC = 0;
        private float tGGL = 0;
        private string partID = string.Empty;
        private string k = string.Empty;
        private DateTime date = DateTime.Now.AddYears(-80);
        private string shift = string.Empty;
        private DateTime thoiHan = DateTime.Now.AddYears(-80);
        private int tinhTrang;
        private string dKmay = string.Empty;
        private string soJig = string.Empty;
        private string ttnc = string.Empty;
        private string dept = string.Empty;
        private string jig = string.Empty;
        private string note = string.Empty;
        private string lastOption = string.Empty;
        private DateTime registerDate = DateTime.Now.AddYears(-80);
        private int _10 = 0;
        private int _20 = 0;
        private int _80 = 0;
        private string via_check1 = string.Empty;
        private string via_check2 = string.Empty;
        private string via_ghi_chu = string.Empty;
        private DateTime fac_NgayHTTheoKH = DateTime.Now.AddYears(-80);
        private int fac_TT = 0;
        private string fac_NGTruoc = string.Empty;
        private string fac_TTFile = string.Empty;
        private bool fac_Phoi = false;
        private bool fac_File = false;
        private bool fac_Dao = false;
        private bool fac_Dc = false;
        private bool fac_Jig = false;
        private int fac_TTTH = 0;
        private bool fac_DaChuanBi = false;
        private int fac_TTGC = 0;
        private string fac_DeXuat = string.Empty;
        private int fac_OK_Old = 0;
        private int fac_NG_Old = 0;
        private string fac_MachineID = string.Empty;
        private string kT_Sup = string.Empty;
        private float kT_TGUKB1 = 0;
        private float kT_TGTC_HT = 0;
        private float kT_TongTG = 0;
        private int kT_TTPhoi = 0;
        private string kT_GiaHang = string.Empty;
        private string kT_KHXN = string.Empty;
        private string kT_LyDoKHTKH = string.Empty;
        private string kT_DgoiTay = string.Empty;
        private DateTime cua_GCTime = DateTime.Now;
        private string cua_Factory = string.Empty;
        private string cua_Size = string.Empty;
        private double cua_TotalTime = 0;
        private double cua_Plantime = 0;
        private string cua_PrepareDate = string.Empty;
        private string cua_MachineGroup = string.Empty;
        private string tongTG = string.Empty;
        private string nesting = string.Empty;
        private string ddPhoi = string.Empty;
        private bool cua_State = false;
        private string viTriGia = string.Empty;
        private bool deleted = false;

        #region
        public string ViTriGia
        {
            get { return viTriGia; }
            set { viTriGia = value; }
        }

        public string Cua_Size
        {
            get { return cua_Size; }
            set { cua_Size = value; }
        }
        public bool Cua_State
        {
            get { return cua_State; }
            set { cua_State = value; }
        }
        public string Cua_MachineGroup
        {
            get { return cua_MachineGroup; }
            set { cua_MachineGroup = value; }
        }
        public string Cua_PrepareDate
        {
            get { return cua_PrepareDate; }
            set { cua_PrepareDate = value; }
        }

        public double Cua_TotalTime
        {
            get { return cua_TotalTime; }
            set { cua_TotalTime = value; }
        }
        public double Cua_Plantime
        {
            get { return cua_Plantime; }
            set { cua_Plantime = value; }
        }
        public string Cua_Factory
        {
            get { return cua_Factory; }
            set { cua_Factory = value; }
        }

        public DateTime Cua_GCTime
        {
            get { return cua_GCTime; }
            set { cua_GCTime = value; }
        }

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public string Order
        {
            get { return order; }
            set { order = value; }
        }
        public string NC
        {
            get { return nC; }
            set { nC = value; }
        }
        public string MayGC
        {
            get { return mayGC; }
            set { mayGC = value; }
        }

        public string MachineID
        {
            get { return machineID; }
            set { machineID = value; }
        }
        public int Slg
        {
            get { return slg; }
            set { slg = value; }
        }
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }
        public DateTime Finish
        {
            get { return finish; }
            set { finish = value; }
        }
        public float TGGC
        {
            get { return tGGC; }
            set { tGGC = value; }
        }
        public float TGGL
        {
            get { return tGGL; }
            set { tGGL = value; }
        }
        public string PartID
        {
            get { return partID; }
            set { partID = value; }
        }
        public string K
        {
            get { return k; }
            set { k = value; }
        }
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public string Shift
        {
            get { return shift; }
            set { shift = value; }
        }
        public DateTime ThoiHan
        {
            get { return thoiHan; }
            set { thoiHan = value; }
        }
        public int TinhTrang
        {
            get { return tinhTrang; }
            set { tinhTrang = value; }
        }
        public string DKmay
        {
            get { return dKmay; }
            set { dKmay = value; }
        }
        public string SoJig
        {
            get { return soJig; }
            set { soJig = value; }
        }
        public string TTNC
        {
            get { return ttnc; }
            set { ttnc = value; }
        }
        public string Dept
        {
            get { return dept; }
            set { dept = value; }
        }
        public string Jig
        {
            get { return jig; }
            set { jig = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string LastOption
        {
            get { return lastOption; }
            set { lastOption = value; }
        }

        public DateTime RegisterDate
        {
            get { return registerDate; }
            set { registerDate = value; }
        }
        public int __10
        {
            get { return _10; }
            set { _10 = value; }
        }
        public int __20
        {
            get { return _20; }
            set { _20 = value; }
        }
        public int __80
        {
            get { return _80; }
            set { _80 = value; }
        }
        public string Via_check1
        {
            get { return via_check1; }
            set { via_check1 = value; }
        }
        public string Via_check2
        {
            get { return via_check2; }
            set { via_check2 = value; }
        }

        public string Via_ghi_chu
        {
            get { return via_ghi_chu; }
            set { via_ghi_chu = value; }
        }

        public DateTime Fac_NgayHTTheoKH
        {
            get { return fac_NgayHTTheoKH; }
            set { fac_NgayHTTheoKH = value; }
        }
        public int Fac_TT
        {
            get { return fac_TT; }
            set { fac_TT = value; }
        }
        public string Fac_NGTruoc
        {
            get { return fac_NGTruoc; }
            set { fac_NGTruoc = value; }
        }
        public string Fac_TTFile
        {
            get { return fac_TTFile; }
            set { fac_TTFile = value; }
        }
        public bool Fac_Phoi
        {
            get { return fac_Phoi; }
            set { fac_Phoi = value; }
        }
        public bool Fac_File
        {
            get { return fac_File; }
            set { fac_File = value; }
        }
        public bool Fac_Dao
        {
            get { return fac_Dao; }
            set { fac_Dao = value; }
        }

        public bool Fac_Dc
        {
            get { return fac_Dc; }
            set { fac_Dc = value; }
        }

        public bool Fac_Jig
        {
            get { return fac_Jig; }
            set { fac_Jig = value; }
        }
        public int Fac_TTTH
        {
            get { return fac_TTTH; }
            set { fac_TTTH = value; }
        }
        public bool Fac_DaChuanBi
        {
            get { return fac_DaChuanBi; }
            set { fac_DaChuanBi = value; }
        }
        public int Fac_TTGC
        {
            get { return fac_TTGC; }
            set { fac_TTGC = value; }
        }
        public string Fac_DeXuat
        {
            get { return fac_DeXuat; }
            set { fac_DeXuat = value; }
        }
        public int Fac_OK_Old
        {
            get { return fac_OK_Old; }
            set { fac_OK_Old = value; }
        }
        public int Fac_NG_Old
        {
            get { return fac_NG_Old; }
            set { fac_NG_Old = value; }
        }

        public string Fac_MachineID
        {
            get { return fac_MachineID; }
            set { fac_MachineID = value; }
        }
        public string KT_Sup
        {
            get { return kT_Sup; }
            set { kT_Sup = value; }
        }
        public float KT_TGTC_HT
        {
            get { return kT_TGTC_HT; }
            set { kT_TGTC_HT = value; }
        }

        public float KT_TGUKB1
        {
            get { return kT_TGUKB1; }
            set { kT_TGUKB1 = value; }
        }
        public float KT_TongTG
        {
            get { return kT_TongTG; }
            set { kT_TongTG = value; }
        }
        public int KT_TTPhoi
        {
            get { return kT_TTPhoi; }
            set { kT_TTPhoi = value; }
        }
        public string KT_GiaHang
        {
            get { return kT_GiaHang; }
            set { kT_GiaHang = value; }
        }
        public string KT_KHXN
        {
            get { return kT_KHXN; }
            set { kT_KHXN = value; }
        }
        public string KT_LyDoKHTKH
        {
            get { return kT_LyDoKHTKH; }
            set { kT_LyDoKHTKH = value; }
        }
        public string KT_DgoiTay
        {
            get { return kT_DgoiTay; }
            set { kT_DgoiTay = value; }
        }

        public string TongTG
        {
            get { return tongTG; }
            set { tongTG = value; }
        }

        public string Nesting
        {
            get { return nesting; }
            set { nesting = value; }
        }

        public string Phoidd
        {
            get { return ddPhoi; }
            set { ddPhoi = value; }
        }

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
        #endregion
    }
}
