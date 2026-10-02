namespace TechMartManager.Models;

public class SanPham
{
    public string  MaSP        { get; set; } = "";
    public string  TenSP       { get; set; } = "";
    public string  DanhMuc     { get; set; } = "";
    public decimal DonGia      { get; set; }
    public int     SoLuong     { get; set; }
    public string  DuongDanAnh { get; set; } = "";
}

public class DanhMucItem
{
    public string Ten  { get; set; } = "";
    public string MaDM { get; set; } = "";
    public override string ToString() => Ten;
}
