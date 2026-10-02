using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSpeedLogistics
{
    // ==========================================
    // A. ABSTRACT CLASS PHUONGTIEN (Lớp cha trừu tượng)
    // ==========================================
    public abstract class PhuongTien
    {
        private string _maPT = "PT000";
        private string _tenHang = string.Empty;
        private int _namSanXuat;
        private decimal _giaGoc;

        public string MaPT
        {
            get => _maPT;
            set => _maPT = string.IsNullOrWhiteSpace(value) ? "PT000" : value.Trim();
        }

        public string TenHang
        {
            get => _tenHang;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên hãng không được để trống!");
                _tenHang = value.Trim();
            }
        }

        public int NamSanXuat
        {
            get => _namSanXuat;
            set
            {
                int currentYear = DateTime.Now.Year;
                if (value < 1900 || value > currentYear)
                    throw new ArgumentException("Năm sản xuất không hợp lệ! (Phải từ 1900 đến năm hiện tại)");
                _namSanXuat = value;
            }
        }

        public decimal GiaGoc
        {
            get => _giaGoc;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Giá gốc phải lớn hơn 0!");
                _giaGoc = value;
            }
        }

        public PhuongTien(string maPT, string tenHang, int namSanXuat, decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }

        public abstract decimal TinhGiaLanBanh();

        public virtual string GetInfo()
        {
            return $"Mã PT: {MaPT} | Hãng: {TenHang} | Năm SX: {NamSanXuat} | Giá gốc: {GiaGoc:N0} VNĐ";
        }
    }

    // ==========================================
    // B. CLASS OTO (Kế thừa từ PhuongTien)
    // ==========================================
    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get => _soChoNgoi;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số chỗ ngồi phải lớn hơn 0!");
                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get => _dungTichDongCo;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích động cơ phải lớn hơn 0!");
                _dungTichDongCo = value;
            }
        }

        public OTo(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int soChoNgoi, double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {
                decimal thueTruocBa = GiaGoc * 0.12m;
                decimal thueTTDB = GiaGoc * 0.30m;
                return GiaGoc + thueTruocBa + thueTTDB;
            }
            else
            {
                decimal thueTruocBa = GiaGoc * 0.10m;
                return GiaGoc + thueTruocBa;
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | Loại: Ô tô | Số chỗ: {SoChoNgoi} | Dung tích ĐC: {DungTichDongCo}L | Giá lăn bánh: {TinhGiaLanBanh():N0} VNĐ";
        }
    }

    // ==========================================
    // C. CLASS XEMAY (Kế thừa từ PhuongTien)
    // ==========================================
    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get => _dungTichXylanh;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích xi-lanh phải lớn hơn 0!");
                _dungTichXylanh = value;
            }
        }

        public XeMay(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
            {
                return GiaGoc + (GiaGoc * 0.02m);
            }
            else
            {
                return GiaGoc + (GiaGoc * 0.05m);
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | Loại: Xe máy | Dung tích Xylanh: {DungTichXylanh}cc | Giá lăn bánh: {TinhGiaLanBanh():N0} VNĐ";
        }
    }

    // ==========================================
    // D. CLASS QUANLYPHUONGTIEN (Quản lý tập hợp)
    // ==========================================
    public class QuanLyPhuongTien
    {
        private readonly List<PhuongTien> _danhSach = new List<PhuongTien>();

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt != null)
            {
                _danhSach.Add(pt);
                Console.WriteLine("=> Thêm phương tiện thành công!");
            }
        }

        public void DisplayAll()
        {
            Console.WriteLine("\n================ DANH SÁCH PHƯƠNG TIỆN ================");
            if (_danhSach.Count == 0)
            {
                Console.WriteLine("Danh sách hiện tại đang trống!");
                return;
            }

            foreach (var pt in _danhSach)
            {
                Console.WriteLine(pt.GetInfo());
            }
            Console.WriteLine("=======================================================");
        }

        public PhuongTien? FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0) return null;
            return _danhSach.OrderByDescending(pt => pt.TinhGiaLanBanh()).FirstOrDefault();
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<PhuongTien>();

            return _danhSach
                .Where(pt => pt.TenHang.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    // ==========================================
    // KỊCH BẢN KIỂM THỬ NHẬP TỪ BÀN PHÍM (INTERACTIVE TEST RUNNER)
    // ==========================================
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var quanLy = new QuanLyPhuongTien();

            Console.WriteLine("===================================================================");
            Console.WriteLine("   HỆ THỐNG QUẢN LÝ PHƯƠNG TIỆN GIAO THÔNG - AUTOSPEED LOGISTICS   ");
            Console.WriteLine("===================================================================");

            // -------------------------------------------------------------
            // TC01: THỰC HÀNH KIỂM TRA VALIDATION NĂM SẢN XUẤT BẰNG CÁCH NHẬP SAI
            // -------------------------------------------------------------
            Console.WriteLine("\n--- [TEST CASE 01]: KIỂM TRA VALIDATION NĂM SẢN XUẤT ---");
            Console.WriteLine("(Gợi ý: Thử nhập năm không hợp lệ như 1850 hoặc năm lớn hơn năm hiện tại)");

            try
            {
                Console.Write("Nhập mã phương tiện: ");
                string ma = Console.ReadLine()!;
                Console.Write("Nhập tên hãng: ");
                string hang = Console.ReadLine()!;
                Console.Write("Nhập năm sản xuất: ");
                int nam = int.Parse(Console.ReadLine()!);
                Console.Write("Nhập giá gốc (VNĐ): ");
                decimal gia = decimal.Parse(Console.ReadLine()!);

                var otoTest = new OTo(ma, hang, nam, gia, 5, 2.0);
                Console.WriteLine("[KẾT QUẢ]: Khởi tạo thành công đối tượng!");
                quanLy.AddPhuongTien(otoTest);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[PASSED TC01 - BẮT BẪY THÀNH CÔNG]: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("[LỖI]: Kiểu dữ liệu nhập vào không hợp lệ!");
            }

            // -------------------------------------------------------------
            // TC02: NHẬP DỮ LIỆU ĐỂ KIỂM TRA TÍNH GIÁ LĂN BÁNH Ô TÔ
            // -------------------------------------------------------------
            Console.WriteLine("\n--- [TEST CASE 02]: NHẬP DỮ LIỆU Ô TÔ ---");
            Console.WriteLine("(Gợi ý: Nhập Số chỗ = 5, Giá gốc = 1000000000 để kiểm tra kết quả 1.42 tỷ VNĐ)");
            try
            {
                Console.Write("Nhập mã Ô tô (VD: OT001): ");
                string maOTo = Console.ReadLine()!;
                Console.Write("Nhập tên hãng Ô tô: ");
                string hangOTo = Console.ReadLine()!;
                Console.Write("Nhập năm sản xuất: ");
                int namOTo = int.Parse(Console.ReadLine()!);
                Console.Write("Nhập giá gốc (VNĐ): ");
                decimal giaOTo = decimal.Parse(Console.ReadLine()!);
                Console.Write("Nhập số chỗ ngồi: ");
                int soCho = int.Parse(Console.ReadLine()!);
                Console.Write("Nhập dung tích động cơ (Lít): ");
                double dungTichDC = double.Parse(Console.ReadLine()!);

                var oto = new OTo(maOTo, hangOTo, namOTo, giaOTo, soCho, dungTichDC);
                quanLy.AddPhuongTien(oto);
                Console.WriteLine($"==> Giá lăn bánh tính được: {oto.TinhGiaLanBanh():N0} VNĐ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LỖI]: {ex.Message}");
            }

            // -------------------------------------------------------------
            // TC03: NHẬP DỮ LIỆU ĐỂ KIỂM TRA TÍNH GIÁ LĂN BÁNH XE MÁY
            // -------------------------------------------------------------
            Console.WriteLine("\n--- [TEST CASE 03]: NHẬP DỮ LIỆU XE MÁY ---");
            Console.WriteLine("(Gợi ý: Nhập Dung tích = 150cc, Giá gốc = 50000000 để kiểm tra kết quả 51 triệu VNĐ)");
            try
            {
                Console.Write("Nhập mã Xe máy (VD: XM001): ");
                string maXeMay = Console.ReadLine()!;
                Console.Write("Nhập tên hãng Xe máy: ");
                string hangXeMay = Console.ReadLine()!;
                Console.Write("Nhập năm sản xuất: ");
                int namXeMay = int.Parse(Console.ReadLine()!);
                Console.Write("Nhập giá gốc (VNĐ): ");
                decimal giaXeMay = decimal.Parse(Console.ReadLine()!);
                Console.Write("Nhập dung tích xilanh (cc): ");
                int dungTichXL = int.Parse(Console.ReadLine()!);

                var xeMay = new XeMay(maXeMay, hangXeMay, namXeMay, giaXeMay, dungTichXL);
                quanLy.AddPhuongTien(xeMay);
                Console.WriteLine($"==> Giá lăn bánh tính được: {xeMay.TinhGiaLanBanh():N0} VNĐ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LỖI]: {ex.Message}");
            }

            // -------------------------------------------------------------
            // TC04: IN TOÀN BỘ DANH SÁCH PHƯƠNG TIỆN (KIỂM TRA ĐA HÌNH)
            // -------------------------------------------------------------
            Console.WriteLine("\n--- [TEST CASE 04]: KIỂM TRA ĐA HÌNH TRONG LIST<PHUONGTIEN> ---");
            quanLy.DisplayAll();

            // -------------------------------------------------------------
            // TC05: TÌM PHƯƠNG TIỆN CÓ GIÁ LĂN BÁNH CAO NHẤT
            // -------------------------------------------------------------
            Console.WriteLine("\n--- [TEST CASE 05]: TÌM PHƯƠNG TIỆN CÓ GIÁ LĂN BÁNH CAO NHẤT ---");
            var ptMax = quanLy.FindMaxGiaLanBanh();
            if (ptMax != null)
            {
                Console.WriteLine("Phương tiện có Giá Lăn Bánh lớn nhất:");
                Console.WriteLine(ptMax.GetInfo());
            }

            // -------------------------------------------------------------
            // TÌM KIẾM THEO TÊN HÃNG
            // -------------------------------------------------------------
            Console.WriteLine("\n--- KIỂM TRA TÌM KIẾM THEO TÊN HÃNG ---");
            Console.Write("Nhập từ khóa tên hãng cần tìm: ");
            string keyword = Console.ReadLine()!;
            var result = quanLy.SearchByName(keyword);

            if (result.Count > 0)
            {
                Console.WriteLine($"Tìm thấy {result.Count} phương tiện phù hợp:");
                foreach (var pt in result)
                {
                    Console.WriteLine(pt.GetInfo());
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy phương tiện nào phù hợp với từ khóa!");
            }

            Console.WriteLine("\n=== HOÀN THÀNH CHƯƠNG TRÌNH KIỂM THỬ ===");
        }
    }
}