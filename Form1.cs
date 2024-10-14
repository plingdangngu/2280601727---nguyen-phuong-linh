using DE01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DE01
{
    public partial class frmSinhvien : Form
    {
        Model1 context = new Model1();
        public frmSinhvien()
        {
            InitializeComponent();
        }

        private void frmSinhvien_Load(object sender, EventArgs e)
        {
            LoadDanhSachSinhVien();
            LoadDanhSachLop();
        }
        private void LoadDanhSachSinhVien()
        {
            try
            {
                var sinhvienList = context.Sinhviens.ToList();
                dgvSinhvien.DataSource = sinhvienList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sinh viên: " + ex.Message);
            }
        }

        private void LoadDanhSachLop()
        {
            try
            {
                var lopList = context.Lops.ToList();
                cmblop.DataSource = lopList;
                cmblop.DisplayMember = "TenLop";
                cmblop.ValueMember = "MaLop";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách lớp: " + ex.Message);
            }
        }

        private void txtmssv_TextChanged(object sender, EventArgs e)
        {
            // mssv của sinh vien
        }

        private void txthoten_TextChanged(object sender, EventArgs e)
        {
            //họ tên
        }

        private void cmblop_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lớp
        }

        private void dtngaysinh_ValueChanged(object sender, EventArgs e)
        {
            //ngày sinh
        }

        private void btntim_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các ô nhập liệu
            string maSV = txtmssv.Text.Trim();
            string hoten = txthoten.Text.Trim();
            string maLop = cmblop.SelectedValue?.ToString(); // Lấy giá trị MaLop từ combobox

            // Tạo truy vấn tìm kiếm
            var query = context.Sinhviens.AsQueryable(); // Khởi tạo truy vấn

            // Nếu có mã sinh viên, thêm vào truy vấn
            if (!string.IsNullOrEmpty(maSV))
            {
                query = query.Where(sv => sv.MaSV.Contains(maSV));
            }

            // Nếu có họ tên, thêm vào truy vấn
            if (!string.IsNullOrEmpty(hoten))
            {
                query = query.Where(sv => sv.HotenSV.Contains(hoten));
            }

            // Nếu có mã lớp, thêm vào truy vấn
            if (!string.IsNullOrEmpty(maLop))
            {
                query = query.Where(sv => sv.MaLop == maLop);
            }

            // Lấy danh sách sinh viên theo truy vấn
            var result = query.ToList();

            // Hiển thị kết quả trong DataGridView
            dgvSinhvien.DataSource = result;

            // Kiểm tra có tìm thấy sinh viên hay không
            if (result.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sinh viên nào!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnthem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtmssv.Text) ||
                    string.IsNullOrWhiteSpace(txthoten.Text) ||
                    cmblop.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng sinh viên mới
                var newSinhVien = new Sinhvien()
                {
                    MaSV = txtmssv.Text.Trim(),
                    HotenSV = txthoten.Text.Trim(),
                    MaLop = cmblop.SelectedValue.ToString(), // Đảm bảo cmblop chứa giá trị MaLop
                    NgaySinh = dtngaysinh.Value
                };

                // Thêm vào context và lưu thay đổi
                context.Sinhviens.Add(newSinhVien);
                context.SaveChanges();

                // Tải lại danh sách sinh viên
                LoadDanhSachSinhVien();

                // Hiển thị thông báo thành công
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (DbUpdateException dbEx)
            {
                // Xử lý ngoại lệ cụ thể liên quan đến cơ sở dữ liệu
                MessageBox.Show("Lỗi khi thêm sinh viên: " + dbEx.InnerException?.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnxoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có sinh viên nào được chọn trong DataGridView không
                if (dgvSinhvien.SelectedRows.Count > 0)
                {
                    // Lấy mã sinh viên từ dòng đã chọn
                    var selectedRow = dgvSinhvien.SelectedRows[0];
                    string maSV = selectedRow.Cells["MaSV"].Value.ToString(); // Đảm bảo tên cột đúng

                    // Tìm sinh viên trong cơ sở dữ liệu
                    var sinhvienToRemove = context.Sinhviens.Find(maSV);
                    if (sinhvienToRemove != null)
                    {
                        context.Sinhviens.Remove(sinhvienToRemove);
                        context.SaveChanges();

                        // Tải lại danh sách sinh viên
                        LoadDanhSachSinhVien();
                        MessageBox.Show("Xóa sinh viên thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để xóa.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message);
            }
        }


        private void btnsua_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có sinh viên nào được chọn trong DataGridView không
                if (dgvSinhvien.SelectedRows.Count > 0)
                {
                    // Lấy mã sinh viên từ dòng đã chọn
                    var selectedRow = dgvSinhvien.SelectedRows[0];
                    string maSV = selectedRow.Cells["MaSV"].Value.ToString(); // Đảm bảo tên cột đúng

                    // Tìm sinh viên trong cơ sở dữ liệu
                    var sinhvienToUpdate = context.Sinhviens.Find(maSV);
                    if (sinhvienToUpdate != null)
                    {
                        // Cập nhật thông tin sinh viên từ các textbox
                        sinhvienToUpdate.HotenSV = txthoten.Text; // Tên sinh viên
                        sinhvienToUpdate.NgaySinh = dtngaysinh.Value; // Ngày sinh
                        sinhvienToUpdate.MaLop = cmblop.SelectedValue.ToString(); // Mã lớp

                        // Lưu thay đổi
                        context.SaveChanges();

                        // Tải lại danh sách sinh viên
                        LoadDanhSachSinhVien();
                        MessageBox.Show("Cập nhật thông tin sinh viên thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để sửa.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để sửa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa thông tin sinh viên: " + ex.Message);
            }
        }


        private void dgvSinhvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //nơi chứa dữ liệu từ sql
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Nếu người dùng chọn "Yes", thoát ứng dụng
                Application.Exit();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvSinhvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
                // Kiểm tra xem người dùng có nhấp vào hàng hợp lệ không
                if (e.RowIndex >= 0)
                {
                    // Lấy dữ liệu của dòng đã chọn
                    DataGridViewRow row = dgvSinhvien.Rows[e.RowIndex];

                    // Hiển thị dữ liệu lên các control
                    txtmssv.Text = row.Cells["MaSV"].Value.ToString();
                    txthoten.Text = row.Cells["HotenSV"].Value.ToString();
                    cmblop.SelectedValue = row.Cells["MaLop"].Value.ToString();
                    dtngaysinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                }
            

        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã có sinh viên nào được chọn chưa
            if (dgvSinhvien.SelectedRows.Count > 0)
            {
                try
                {
                    // Lấy sinh viên đã chọn
                    var selectedRow = dgvSinhvien.SelectedRows[0];
                    var maSV = selectedRow.Cells["MaSV"].Value.ToString(); // Giả sử MaSV là cột bạn cần để nhận diện sinh viên

                    // Tìm sinh viên trong context bằng MaSV
                    var sinhVienToUpdate = context.Sinhviens.SingleOrDefault(sv => sv.MaSV == maSV);

                    if (sinhVienToUpdate != null)
                    {
                        // Cập nhật thông tin sinh viên
                        sinhVienToUpdate.HotenSV = txthoten.Text;
                        sinhVienToUpdate.MaLop = cmblop.SelectedValue.ToString();
                        sinhVienToUpdate.NgaySinh = dtngaysinh.Value;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        context.SaveChanges();

                        // Tải lại danh sách sinh viên
                        LoadDanhSachSinhVien();

                        // Hiển thị thông báo thành công
                        MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnkluu_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            var result = MessageBox.Show("Bạn có chắc chắn không lưu dữ liệu đã sửa?",
                                           "Xác nhận",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Nếu người dùng chọn "Có", làm mới các control
                txtmssv.Clear();
                txthoten.Clear();
                cmblop.SelectedIndex = -1; // Chọn không có lớp
                dtngaysinh.Value = DateTime.Now; // Hoặc một giá trị mặc định

                MessageBox.Show("Dữ liệu đã được hủy bỏ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Nếu người dùng chọn "Không", không làm gì cả và để lại dữ liệu trên các control.
        }


    }
}
