using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingWork.DataBase
{
    public class clsBusOrder
    {

        #region Members
        protected int id = 0;
        protected string orderCat = string.Empty;
        protected string orderGoc = string.Empty;
        protected string bOderNo = string.Empty;
        protected DateTime date = DateTime.Now;
        protected string planNo = string.Empty;
        protected string partID = string.Empty;
        protected int qty = 0;
        protected DateTime dealine = DateTime.Now;
        protected int rawQty = 0;
        protected int helisertQty = 0;
        protected int blastQty = 0;
        protected string mONo = string.Empty;
        protected int mOQty = 0;
        protected int orderType = 0;
        protected bool temp = false;
        protected bool started = false;
        protected bool finished = false;
        protected string change = string.Empty;
        protected DateTime changeDate = DateTime.Now;
        protected bool imported = false;
        protected string importFrom = string.Empty;
        protected string note = string.Empty;
        protected DateTime finishDate = DateTime.Now;
        protected string moc8chinh = string.Empty;
        protected string noiCat = string.Empty;
        protected DateTime thVatLieu = DateTime.Now;
        protected DateTime thPhoi = DateTime.Now;
        protected bool deleted = false;
        protected string status = string.Empty;
        protected bool paid = false;
        protected DateTime payDate = DateTime.Now.AddYears(50).Date;
        #endregion
        #region Public Properties
        
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Moc8chinh
        {
            get { return moc8chinh; }
            set { moc8chinh = value; }
        }

        public string NoiCat
        {
            get { return noiCat; }
            set { noiCat = value; }
        }

        public DateTime THVatLieu
        {
            get { return thVatLieu; }
            set { thVatLieu = value; }
        }

        public DateTime THPhoi
        {
            get { return thPhoi; }
            set { thPhoi = value; }
        }

        public DateTime FinishDate
        {
            get { return finishDate; }
            set { finishDate = value; }
        }
        /// <summary>
        /// Property relating to database column BOderNo(nvarchar(50),not null)
        /// </summary>
        public string BOderNo
        {
            get { return bOderNo; }
            set { bOderNo = value; }
        }

        /// <summary>
        /// Property relating to database column date(datetime,null)
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Property relating to database column planNo(nchar(10),null)
        /// </summary>
        public string PlanNo
        {
            get { return planNo; }
            set { planNo = value; }
        }

        /// <summary>
        /// Property relating to database column PartID(nvarchar(50),null)
        /// </summary>
        public string PartID
        {
            get { return partID; }
            set { partID = value; }
        }

        /// <summary>
        /// Property relating to database column Qty(int,null)
        /// </summary>
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        /// <summary>
        /// Property relating to database column Dealine(datetime,null)
        /// </summary>
        public DateTime Deadline
        {
            get { return dealine; }
            set { dealine = value; }
        }

        /// <summary>
        /// Property relating to database column RawQty(int,null)
        /// </summary>
        public int RawQty
        {
            get { return rawQty; }
            set { rawQty = value; }
        }

        /// <summary>
        /// Property relating to database column HelisertQty(int,null)
        /// </summary>
        public int HelisertQty
        {
            get { return helisertQty; }
            set { helisertQty = value; }
        }

        /// <summary>
        /// Property relating to database column BlastQty(int,null)
        /// </summary>
        public int BlastQty
        {
            get { return blastQty; }
            set { blastQty = value; }
        }

        /// <summary>
        /// Property relating to database column MONo(nvarchar(50),null)
        /// </summary>
        public string MONo
        {
            get { return mONo; }
            set { mONo = value; }
        }

        /// <summary>
        /// Property relating to database column MOQty(int,null)
        /// </summary>
        public int MOQty
        {
            get { return mOQty; }
            set { mOQty = value; }
        }

        
        public int OrderType
        {
            get { return orderType; }
            set { orderType = value; }
        }

        public bool TempOrder
        {
            get { return temp; }
            set { temp = value; }
        }

        
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public bool Paid
        {
            get {
                if (!Finished)
                {
                    return false;
                }
                return Paid;
            }
            set { paid = value; }
        }

        public DateTime PayDate
        {
            get {
                if (!Paid)
                {
                    return DateTime.Now.AddYears(50).Date;
                }

                return payDate;
            }
            set { payDate = value; }
        }

        /// <summary>
        /// Property relating to database column Started(bit,null)
        /// </summary>
        public bool Started
        {
            get { return started; }
            set { started = value; }
        }

        /// <summary>
        /// Property relating to database column Finished(bit,null)
        /// </summary>
        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }

        /// <summary>
        /// Property relating to database column Change(nvarchar(50),null)
        /// </summary>
        public string Change
        {
            get { return change; }
            set { change = value; }
        }

        /// <summary>
        /// Property relating to database column ChangeDate(datetime,null)
        /// </summary>
        public DateTime ChangeDate
        {
            get { return changeDate; }
            set { changeDate = value; }
        }

        /// <summary>
        /// Property relating to database column Imported(bit,null)
        /// </summary>
        public bool Imported
        {
            get { return imported; }
            set { imported = value; }
        }

        /// <summary>
        /// Property relating to database column ImportFrom(nvarchar(50),null)
        /// </summary>
        public string ImportFrom
        {
            get { return importFrom; }
            set { importFrom = value; }
        }

        /// <summary>
        /// Property relating to database column Note(nvarchar(50),null)
        /// </summary>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string OrderGoc
        {
            get { return orderGoc; }
            set { orderGoc = value; }
        }

        public string OrderCat
        {
            get { return orderCat; }
            set { orderCat = value; }
        }

        #endregion


    }
}
