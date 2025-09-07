using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT_Software
{
    public partial class Frm_QuaySo : Form
    {
        private int giaiQquay = 0;
        private bool nhaySo = false;
        private List<GiaiThuong> giai = new List<GiaiThuong>();
        private List<DanhSachNV> DSNV = new List<DanhSachNV>();
        public Frm_QuaySo()
        {
            InitializeComponent();
            giai.Add(new GiaiThuong() { Index = 1, SoGiai = 1 ,LabelIndex = 2 });
            giai.Add(new GiaiThuong() { Index = 2, SoGiai = 2 ,LabelIndex = 4});
            giai.Add(new GiaiThuong() { Index = 3, SoGiai = 3 ,LabelIndex = 7});
            giai.Add(new GiaiThuong() { Index = 4, SoGiai = 5 ,LabelIndex = 11});
            giai.Add(new GiaiThuong() { Index = 5, SoGiai = 10,LabelIndex = 17 });
            label2.Text = "????";
            label4.Text = "????";
            label5.Text = "????";
            label7.Text = "????";
            label8.Text = "????";
            label9.Text = "????";
            label11.Text = "????";
            label12.Text = "????";
            label13.Text = "????";
            label14.Text = "????";
            label15.Text = "????";
            label17.Text = "????";
            label18.Text = "????";
            label19.Text = "????";
            label20.Text = "????";
            label21.Text = "????";
            label22.Text = "????";
            label23.Text = "????";
            label24.Text = "????";
            label25.Text = "????";
            label26.Text = "????";
            DSNV.Add(new DanhSachNV() { Code = "0002", Name = "	Vũ Văn Vĩ              "});
            DSNV.Add(new DanhSachNV() { Code = "0004", Name = "	Vũ Thị Mơ			   "});
            DSNV.Add(new DanhSachNV() { Code = "0005", Name = "	Nguyễn Bách Hiệp	   "});
            DSNV.Add(new DanhSachNV() { Code = "0006", Name = "	Trần Văn Mạnh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0007", Name = "	Hoàng Văn Chung		   "});
            DSNV.Add(new DanhSachNV() { Code = "0010", Name = "	Nguyễn Văn Thuấn	   "});
            DSNV.Add(new DanhSachNV() { Code = "0012", Name = "	Nguyễn Đăng Hòa		   "});
            DSNV.Add(new DanhSachNV() { Code = "0013", Name = "	Bùi  Ngọc Diệp		   "});
            DSNV.Add(new DanhSachNV() { Code = "0015", Name = "	Vũ Thị Toán			   "});
            DSNV.Add(new DanhSachNV() { Code = "0017", Name = "	Phạm Văn Thưởng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0019", Name = "	Nguyễn Văn Thinh	   "});
            DSNV.Add(new DanhSachNV() { Code = "0021", Name = "	Vũ Thị Nga			   "});
            DSNV.Add(new DanhSachNV() { Code = "0022", Name = "	Nguyễn Văn Cường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0023", Name = "	Nguyễn Thị Như Hoa	   "});
            DSNV.Add(new DanhSachNV() { Code = "0024", Name = "	Bùi Văn Hậu			   "});
            DSNV.Add(new DanhSachNV() { Code = "0026", Name = "	Đỗ Văn Quân			   "});
            DSNV.Add(new DanhSachNV() { Code = "0027", Name = "	Nguyễn Việt Quang	   "});
            DSNV.Add(new DanhSachNV() { Code = "0029", Name = "	Bùi Thị Toan		   "});
            DSNV.Add(new DanhSachNV() { Code = "0035", Name = "	Trương Văn Quang	   "});
            DSNV.Add(new DanhSachNV() { Code = "0036", Name = "	Trần Văn Anh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0038", Name = "	Vũ Quang Khôi		   "});
            DSNV.Add(new DanhSachNV() { Code = "0041", Name = "	Nguyễn Danh Công	   "});
            DSNV.Add(new DanhSachNV() { Code = "0043", Name = "	Vũ Đức Hoàn  		   "});
            DSNV.Add(new DanhSachNV() { Code = "0045", Name = "	Nguyễn Văn Tuấn 	   "});
            DSNV.Add(new DanhSachNV() { Code = "0046", Name = "	Tống Thị Trang		   "});
            DSNV.Add(new DanhSachNV() { Code = "0047", Name = "	Nguyễn Văn Tuấn		   "});
            DSNV.Add(new DanhSachNV() { Code = "0048", Name = "	Vũ Xuân Nguyên		   "});
            DSNV.Add(new DanhSachNV() { Code = "0049", Name = "	Hoàng Thị Tuất		   "});
            DSNV.Add(new DanhSachNV() { Code = "0050", Name = "	Vũ Thị Thoan		   "});
            DSNV.Add(new DanhSachNV() { Code = "0051", Name = "	Nguyễn Thị Lệ		   "});
            DSNV.Add(new DanhSachNV() { Code = "0055", Name = "	Trần Thị Thu Huyền	   "});
            DSNV.Add(new DanhSachNV() { Code = "0057", Name = "	Nguyễn Văn Quốc		   "});
            DSNV.Add(new DanhSachNV() { Code = "0062", Name = "	Nguyễn Thị Mỹ		   "});
            DSNV.Add(new DanhSachNV() { Code = "0064", Name = "	Tiêu Thị Như		   "});
            DSNV.Add(new DanhSachNV() { Code = "0065", Name = "	Nguyễn Văn Thắng	   "});
            DSNV.Add(new DanhSachNV() { Code = "0066", Name = "	Phạm Thị Nhiên		   "});
            DSNV.Add(new DanhSachNV() { Code = "0070", Name = "	Trần Thị Hiền		   "});
            DSNV.Add(new DanhSachNV() { Code = "0071", Name = "	Ngô Văn Đức			   "});
            DSNV.Add(new DanhSachNV() { Code = "0073", Name = "	Trần Ngọc Anh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0084", Name = "	Phạm Hữu Sơn		   "});
            DSNV.Add(new DanhSachNV() { Code = "0087", Name = "	Phạm Văn Khuynh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0088", Name = "	Trần Thị Nhàn		   "});
            DSNV.Add(new DanhSachNV() { Code = "0091", Name = "	Nguyễn Thị Vân  k.toán "});
            DSNV.Add(new DanhSachNV() { Code = "0092", Name = "	Nguyễn Quốc Khánh	   "});
            DSNV.Add(new DanhSachNV() { Code = "0099", Name = "	Nguyễn Đức Trường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0100", Name = "	Đinh Hồng Phong		   "});
            DSNV.Add(new DanhSachNV() { Code = "0103", Name = "	Trần Thị Yến		   "});
            DSNV.Add(new DanhSachNV() { Code = "0107", Name = "	Nguyễn Văn Trường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0112", Name = "	Nguyễn Tiến Đạt		   "});
            DSNV.Add(new DanhSachNV() { Code = "0113", Name = "	Nguyễn Phúc Thuần	   "});
            DSNV.Add(new DanhSachNV() { Code = "0114", Name = "	Nguyễn Văn Nguyên	   "});
            DSNV.Add(new DanhSachNV() { Code = "0116", Name = "	Trần Văn Tạo		   "});
            DSNV.Add(new DanhSachNV() { Code = "0124", Name = "	Lê Văn Nam			   "});
            DSNV.Add(new DanhSachNV() { Code = "0133", Name = "	Nguyễn Đình Dân		   "});
            DSNV.Add(new DanhSachNV() { Code = "0137", Name = "	Đào Mạnh Huân		   "});
            DSNV.Add(new DanhSachNV() { Code = "0151", Name = "	Vũ Thị Thảo			   "});
            DSNV.Add(new DanhSachNV() { Code = "0153", Name = "	Hoàng Vũ Quang		   "});
            DSNV.Add(new DanhSachNV() { Code = "0156", Name = "	Vũ Thị Năm			   "});
            DSNV.Add(new DanhSachNV() { Code = "0158", Name = "	Trần Thị Hồng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0161", Name = "	Ninh Văn Anh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0164", Name = "	Nguyễn Văn Trọng 	   "});
            DSNV.Add(new DanhSachNV() { Code = "0165", Name = "	Mạc Trọng Dũng 		   "});
            DSNV.Add(new DanhSachNV() { Code = "0169", Name = "	Dương Thị Kiều Oanh	   "});
            //DSNV.Add(new DanhSachNV() { Code = "0170", Name = "	Phạm Văn Thoòng		   "});
            //DSNV.Add(new DanhSachNV() { Code = "0171", Name = "	Nguyễn Vũ Hậu		   "});
            DSNV.Add(new DanhSachNV() { Code = "0172", Name = "	Hoàng Tùng			   "});
            DSNV.Add(new DanhSachNV() { Code = "0175", Name = "	Nguyễn Thị Thúy		   "});
            DSNV.Add(new DanhSachNV() { Code = "0187", Name = "	Đồng Văn Tuấn		   "});
            DSNV.Add(new DanhSachNV() { Code = "0190", Name = "	Vũ Tiến Thọ			   "});
            DSNV.Add(new DanhSachNV() { Code = "0191", Name = "	Nguyễn Danh Dương	   "});
            DSNV.Add(new DanhSachNV() { Code = "0194", Name = "	Nguyễn Văn Chính	   "});
            DSNV.Add(new DanhSachNV() { Code = "0199", Name = "	Nguyễn Mạnh Toàn	   "});
            DSNV.Add(new DanhSachNV() { Code = "0200", Name = "	Vũ Văn Ban			   "});
            DSNV.Add(new DanhSachNV() { Code = "0201", Name = "	Nguyễn Khắc Thủy	   "});
            DSNV.Add(new DanhSachNV() { Code = "0206", Name = "	Nguyễn Việt Chiến	   "});
            DSNV.Add(new DanhSachNV() { Code = "0209", Name = "	Đặng Văn Vũ			   "});
            DSNV.Add(new DanhSachNV() { Code = "0214", Name = "	Phạm Thị Hương		   "});
            DSNV.Add(new DanhSachNV() { Code = "0217", Name = "	Vũ Đình Hoan		   "});
            DSNV.Add(new DanhSachNV() { Code = "0218", Name = "	Nguyễn Văn Cường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0234", Name = "	Phạm Thị Hìu		   "});
            DSNV.Add(new DanhSachNV() { Code = "0236", Name = "	Hoàng Vũ Quang		   "});
            DSNV.Add(new DanhSachNV() { Code = "0240", Name = "	Đỗ Tuấn Anh			   "});
            DSNV.Add(new DanhSachNV() { Code = "0266", Name = "	Ngô Quang Hùng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0269", Name = "	Vũ Viết Quyền		   "});
            DSNV.Add(new DanhSachNV() { Code = "0270", Name = "	Đỗ Văn Phong		   "});
            DSNV.Add(new DanhSachNV() { Code = "0271", Name = "	Phạm Tiến Đạt		   "});
            DSNV.Add(new DanhSachNV() { Code = "0272", Name = "	Đỗ Thị Thúy Mai		   "});
            DSNV.Add(new DanhSachNV() { Code = "0276", Name = "	Nguyễn Mạnh Tùng	   "});
            DSNV.Add(new DanhSachNV() { Code = "0279", Name = "	Bùi Thị Thu			   "});
            DSNV.Add(new DanhSachNV() { Code = "0288", Name = "	Nguyễn Giang Nam	   "});
            DSNV.Add(new DanhSachNV() { Code = "0298", Name = "	Vũ Việt Kha 		   "});
            DSNV.Add(new DanhSachNV() { Code = "0323", Name = "	Đỗ Thị Hường		   "});
            DSNV.Add(new DanhSachNV() { Code = "0327", Name = "	Bùi Văn Lâm			   "});
            DSNV.Add(new DanhSachNV() { Code = "0341", Name = "	Nguyễn Đình Trường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0344", Name = "	Nguyễn Thị Xuân		   "});
            DSNV.Add(new DanhSachNV() { Code = "0346", Name = "	Đỗ Quang Dũng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0351", Name = "	Vũ Thúy Quỳnh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0361", Name = "	Trần Văn Tài		   "});
            DSNV.Add(new DanhSachNV() { Code = "0367", Name = "	Bùi Văn Thúy		   "});
            DSNV.Add(new DanhSachNV() { Code = "0369", Name = "	Lưu Thị  Thương		   "});
            DSNV.Add(new DanhSachNV() { Code = "0372", Name = "	Trần Văn Luân		   "});
            DSNV.Add(new DanhSachNV() { Code = "0375", Name = "	Trần Quang Tuyên	   "});
            DSNV.Add(new DanhSachNV() { Code = "0377", Name = "	Nguyễn Thị Hương	   "});
            DSNV.Add(new DanhSachNV() { Code = "0378", Name = "	Phạm Văn Phước		   "});
            DSNV.Add(new DanhSachNV() { Code = "0380", Name = "	Phạm Đức Bậc		   "});
            DSNV.Add(new DanhSachNV() { Code = "0384", Name = "	Nguyễn Văn Giỏi		   "});
            DSNV.Add(new DanhSachNV() { Code = "0385", Name = "	Nguyễn Thị Nhài		   "});
            DSNV.Add(new DanhSachNV() { Code = "0395", Name = "	Nguyễn Anh Phong	   "});
            DSNV.Add(new DanhSachNV() { Code = "0401", Name = "	Trần Văn Nam 		   "});
            DSNV.Add(new DanhSachNV() { Code = "0406", Name = "	Đồng Thị Nhuận		   "});
            DSNV.Add(new DanhSachNV() { Code = "0409", Name = "	Mạc Trọng Ánh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0410", Name = "	Nguyễn Tiến Cường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0416", Name = "	Trần Văn Hà			   "});
            DSNV.Add(new DanhSachNV() { Code = "0427", Name = "	Nguyễn Văn Thông	   "});
            DSNV.Add(new DanhSachNV() { Code = "0431", Name = "	Lò Thị Quê			   "});
            DSNV.Add(new DanhSachNV() { Code = "0435", Name = "	Nguyễn Tất Vinh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0445", Name = "	Phạm Văn Nhiêu		   "});
            DSNV.Add(new DanhSachNV() { Code = "0448", Name = "	Trần Hữu Năng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0451", Name = "	Trần Văn Tú			   "});
            DSNV.Add(new DanhSachNV() { Code = "0454", Name = "	Lê Văn Toản			   "});
            DSNV.Add(new DanhSachNV() { Code = "0455", Name = "	Đoàn Văn Hoàng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0456", Name = "	Hoàng Văn Tuấn		   "});
            DSNV.Add(new DanhSachNV() { Code = "0461", Name = "	Nguyễn Thị Linh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0462", Name = "	Phạm Duy Tú			   "});
            DSNV.Add(new DanhSachNV() { Code = "0466", Name = "	Trương Thị Thơm Hương  "});
            DSNV.Add(new DanhSachNV() { Code = "0467", Name = "	Vũ Đức Việt			   "});
            DSNV.Add(new DanhSachNV() { Code = "0473", Name = "	Nguyễn Đức Xuân 	   "});
            DSNV.Add(new DanhSachNV() { Code = "0476", Name = "	Nguyễn Văn Cường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0482", Name = "	Hoàng Thị Hương		   "});
            DSNV.Add(new DanhSachNV() { Code = "0484", Name = "	Trần Hữu Lợi		   "});
            DSNV.Add(new DanhSachNV() { Code = "0486", Name = "	Trần Minh Đức		   "});
            DSNV.Add(new DanhSachNV() { Code = "0489", Name = "	Trần Thị Hoa		   "});
            DSNV.Add(new DanhSachNV() { Code = "0493", Name = "	Nguyễn Hữu Lâm		   "});
            DSNV.Add(new DanhSachNV() { Code = "0494", Name = "	Vũ Văn Hoàng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0496", Name = "	Nguyễn Thượng Quận	   "});
            DSNV.Add(new DanhSachNV() { Code = "0501", Name = "	Nguyễn Thị Hồng Ngọc   "});
            DSNV.Add(new DanhSachNV() { Code = "0504", Name = "	Nguyễn Thành Luân	   "});
            DSNV.Add(new DanhSachNV() { Code = "0507", Name = "	Nguyễn Thanh Tùng	   "});
            DSNV.Add(new DanhSachNV() { Code = "0508", Name = "	Nguyễn Duy Anh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0511", Name = "	Trần Văn Hưng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0513", Name = "	Lê Thị Hương		   "});
            DSNV.Add(new DanhSachNV() { Code = "0518", Name = "	Ngô Văn Vũ			   "});
            DSNV.Add(new DanhSachNV() { Code = "0524", Name = "	Nguyễn Thị Hiền		   "});
            DSNV.Add(new DanhSachNV() { Code = "0525", Name = "	Nguyễn Thị Nhàn		   "});
            DSNV.Add(new DanhSachNV() { Code = "0527", Name = "	Nguyễn Thị Hoàn  	   "});
            DSNV.Add(new DanhSachNV() { Code = "0528", Name = "	Nguyễn Đình Chung 	   "});
            DSNV.Add(new DanhSachNV() { Code = "0529", Name = "	Trần Ngọc Sơn 		   "});
            DSNV.Add(new DanhSachNV() { Code = "0531", Name = "	Nguyễn Thị Thương 	   "});
            DSNV.Add(new DanhSachNV() { Code = "0533", Name = "	Phạm Văn Cẩn		   "});
            DSNV.Add(new DanhSachNV() { Code = "0534", Name = "	Nguyễn Văn Khuyến 	   "});
            DSNV.Add(new DanhSachNV() { Code = "0535", Name = "	Ninh Văn Luân		   "});
            DSNV.Add(new DanhSachNV() { Code = "0536", Name = "	Võ Văn Lịch			   "});
            DSNV.Add(new DanhSachNV() { Code = "0545", Name = "	Chử Minh Khánh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0546", Name = "	Phạm Quang Huy		   "});
            DSNV.Add(new DanhSachNV() { Code = "0552", Name = "	Nguyễn Minh Quân	   "});
            DSNV.Add(new DanhSachNV() { Code = "0554", Name = "	Nguyễn Công Định	   "});
            DSNV.Add(new DanhSachNV() { Code = "0556", Name = "	Phạm Văn Hồng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0557", Name = "	Bùi Văn Nam			   "});
            DSNV.Add(new DanhSachNV() { Code = "0558", Name = "	Đào Văn Hanh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0559", Name = "	Trần Văn Khải		   "});
            DSNV.Add(new DanhSachNV() { Code = "0561", Name = "	Nguyễn Văn Thắng	   "});
            DSNV.Add(new DanhSachNV() { Code = "0563", Name = "	Bùi Quang Huy		   "});
            DSNV.Add(new DanhSachNV() { Code = "0565", Name = "	Nho Khánh Hoàng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0566", Name = "	Nguyễn Đức Chung	   "});
            DSNV.Add(new DanhSachNV() { Code = "0567", Name = "	Dương Văn Quyền		   "});
            DSNV.Add(new DanhSachNV() { Code = "0568", Name = "	Vũ Thị Than			   "});
            DSNV.Add(new DanhSachNV() { Code = "0572", Name = "	Phạm Quang Trường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0573", Name = "	Vũ Thị Gái			   "});
            DSNV.Add(new DanhSachNV() { Code = "0574", Name = "	Vũ Thị Cách			   "});
            DSNV.Add(new DanhSachNV() { Code = "0575", Name = "	Nguyễn Thị Tùy		   "});
            DSNV.Add(new DanhSachNV() { Code = "0576", Name = "	Lù Thanh Trần		   "});
            DSNV.Add(new DanhSachNV() { Code = "0577", Name = "	Nguyễn Văn Trường	   "});
            DSNV.Add(new DanhSachNV() { Code = "0578", Name = "	Nguyễn Hữu Dương	   "});
            DSNV.Add(new DanhSachNV() { Code = "0579", Name = "	Đỗ Trọng Lương		   "});
            DSNV.Add(new DanhSachNV() { Code = "0580", Name = "	Vũ Thị Quỳnh Anh	   "});
            DSNV.Add(new DanhSachNV() { Code = "0581", Name = "	Ninh Hoàng An		   "});
            DSNV.Add(new DanhSachNV() { Code = "0582", Name = "	Nguyễn Văn Anh		   "});
            DSNV.Add(new DanhSachNV() { Code = "0583", Name = "	Vũ Văn Công			   "});
            DSNV.Add(new DanhSachNV() { Code = "0584", Name = "	Vũ Đình Quang		   "});
            DSNV.Add(new DanhSachNV() { Code = "0585", Name = "	Nguyễn Tiến Đạm		   "});
            DSNV.Add(new DanhSachNV() { Code = "0588", Name = "	Đỗ Thị Thu Hương	   "});
            DSNV.Add(new DanhSachNV() { Code = "0589", Name = "	Đỗ Văn Huyên		   "});
            DSNV.Add(new DanhSachNV() { Code = "0591", Name = "	Nguyễn Văn Ý		   "});
            DSNV.Add(new DanhSachNV() { Code = "0592", Name = "	Phạm Đức Việt		   "});
            DSNV.Add(new DanhSachNV() { Code = "0593", Name = "	Nguyễn Ngọc Hà		   "});
            DSNV.Add(new DanhSachNV() { Code = "0594", Name = "	Bùi Thị Huệ			   "});
            DSNV.Add(new DanhSachNV() { Code = "0595", Name = "	Đỗ Nguyễn Long Hải	   "});
            DSNV.Add(new DanhSachNV() { Code = "0596", Name = "	Trần Thị Hồng		   "});
            DSNV.Add(new DanhSachNV() { Code = "0597", Name = "	Đỗ Quang Linh		   "});

        }

        private void Frm_QuaySo_Load(object sender, EventArgs e)
        {
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label16_DoubleClick(object sender, EventArgs e)
        {
            int KKIndex = 5;
            SetGiaiQuay(KKIndex);
        }

        private void SetGiaiQuay(int index)
        {
            giaiQquay = index;
            label28.Text = $@"Giải quay:{index}";
        }

        private void label10_DoubleClick(object sender, EventArgs e)
        {
            SetGiaiQuay(4);
        }

        private void label6_DoubleClick(object sender, EventArgs e)
        {
            SetGiaiQuay(3);
        }

        private void label3_DoubleClick(object sender, EventArgs e)
        {
            SetGiaiQuay(2);
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            SetGiaiQuay(1);
            //nhaySo = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!nhaySo)
            {
                nhaySo = true;
                Thread threadNhaySo = new Thread(new ThreadStart(() => {
                    NhaySo();
                }));
                threadNhaySo.IsBackground = true;
                threadNhaySo.Start();
            }

            Thread threadQuaySo = new Thread(new ThreadStart(() => {
                QuaySo();
            }));
            threadQuaySo.IsBackground = true;
            threadQuaySo.Start();
        }

        private int RandomNumber()
        {
            Random rand = new Random();
            int number = rand.Next(0, DSNV.Count-1);
            return number;
        }

        private void NhaySo()
        {
            while (true)
            {
                if (!nhaySo)
                {
                    break;
                }

                int so = RandomNumber();
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        label27.Text = DSNV[so].Code;
                    }));
                }
                else
                {
                    label27.Text = DSNV[so].Code;
                }
            }
        }

        private void QuaySo()
        {
            var obj = giai.Where(x => x.Index == giaiQquay).FirstOrDefault();
            if(obj == null)
            {
                nhaySo = false;
                return;
            }

            for(int i =0; i< obj.SoGiai; i++)
            {
                Thread.Sleep(3000);
                int num = RandomNumber();
                Control ctn = this.tableLayoutPanel1.Controls[$@"label{obj.LabelIndex+i}"];
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        ctn.Text = DSNV[num].Code;
                    }));
                }
                else
                {
                    ctn.Text = DSNV[num].Code;
                }
                DSNV.RemoveAt(num);
            }

            nhaySo = false;
        }

        private void SingleJackpot(object sender)
        {
            if (!nhaySo)
            {
                nhaySo = true;
                Thread threadNhaySo = new Thread(new ThreadStart(() => {
                    NhaySo();
                }));
                threadNhaySo.IsBackground = true;
                threadNhaySo.Start();
            }

            Thread threadQuaySo = new Thread(new ThreadStart(() => {
                QuaySo(sender);
            }));
            threadQuaySo.IsBackground = true;
            threadQuaySo.Start();
        }

        private void QuaySo(object sender)
        {
            Label label = (Label)sender;
            Thread.Sleep(3000);
            int num = RandomNumber();
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    label.Text = DSNV[num].Code;
                }));
            }
            else
            {
                label.Text = DSNV[num].Code;
            }
            DSNV.RemoveAt(num);
            nhaySo = false;
        }

        private void label4_DoubleClick(object sender, EventArgs e)
        {
            SingleJackpot(sender);
        }
    }

    public class GiaiThuong
    {
        public int Index { get; set; }
        public int SoGiai { get; set; }
        public int LabelIndex { get; set; }
    }

    public class DanhSachNV
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
