using buoi_5;
using BUS;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace buoi_6___mo_hinh_3_lop
{
    public partial class Form1 : Form
    {
        private readonly StudentService studentService = new StudentService();
        public Form1()
        {
            InitializeComponent();
           
        }

        public void Add()
        {

            string mssv = txbId.Text.Trim();
            string tenSinhVien = txbName.Text.Trim();


            if (!float.TryParse(txbAverage.Text, out float diemTB))
            {
                MessageBox.Show("Điểm trung bình phải là số.");
                return;
            }


            if (cbbFaculty.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn khoa.");
                return;
            }


            int facultyId = (int)cbbFaculty.SelectedValue;

            if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(tenSinhVien))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }


            var studentService = new StudentService();


            bool isAdded = studentService.AddStudent(mssv, tenSinhVien, facultyId, diemTB);


            if (isAdded)
            {

                txbId.Clear();
                txbName.Clear();
                txbAverage.Clear();

                MessageBox.Show("Thêm sinh viên thành công");
            }
            else
            {

                MessageBox.Show("Thêm sinh viên không thành công. Vui lòng kiểm tra lại.");
            }
        }

        public void LoadData(bool showStudentsWithNoMajor = false)
        {
            using (var db = new Model1())
            {

                var faculties = db.Faculties.Select(f => new { f.facultyId, f.facultyName }).ToList();
                cbbFaculty.DataSource = faculties;
                cbbFaculty.DisplayMember = "facultyName";
                cbbFaculty.ValueMember = "facultyId";


                List<Student> students;

                if (checkBox1.Checked)
                {
                    students = db.Students.Where(s => s.Major == null).ToList();
                }
                else
                {
                    students = db.Students.ToList();
                }

                var fillStudent = students.Select(s => new
                {
                    mssv = s.mssv,
                    tenSinhVien = s.tenSinhVien,
                    facultyName = s.Faculty.facultyName, // Tên khoa
                    diemTB = s.diemTB,
                    majorName = s.Major != null ? s.Major.majorName : "Chưa có chuyên ngành" ,
                    imagePath =s.imagePath,
                }).ToList();


                dataGridView1.DataSource = fillStudent;


                dataGridView1.Columns["mssv"].HeaderText = "MSSV";
                dataGridView1.Columns["tenSinhVien"].HeaderText = "Tên Sinh Viên";
                dataGridView1.Columns["diemTB"].HeaderText = "Điểm TB";
                dataGridView1.Columns["facultyName"].HeaderText = "Tên Khoa";
                dataGridView1.Columns["majorName"].HeaderText = "Tên Chuyên Ngành";
                dataGridView1.Columns["imagePath"].Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData(); 
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Add();
            Form1_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                string mssv = dataGridView1.SelectedRows[0].Cells["mssv"].Value.ToString();


                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?",
                                                    "Xác nhận xóa",
                                                    MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {

                    studentService.DeleteStudent(mssv);

                    MessageBox.Show("Xóa sinh viên thành công");


                    Form1_Load(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn Ảnh Sinh Viên";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Hiển thị ảnh trong PictureBox
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                    pictureBox1.Tag = openFileDialog.FileName; // Lưu đường dẫn ảnh vào Tag
                }
            }
        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Application.OpenForms["Form2"] == null)
            {
             
                Form2 registerMajorForm = new Form2();

              
                registerMajorForm.Show();
            }
            else
            {
           
                Application.OpenForms["Form2"].BringToFront();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add();
            Form1_Load(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                string mssv = dataGridView1.SelectedRows[0].Cells["mssv"].Value.ToString();


                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?",
                                                    "Xác nhận xóa",
                                                    MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {

                    studentService.DeleteStudent(mssv);

                    MessageBox.Show("Xóa sinh viên thành công");


                    Form1_Load(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
            }
        }

        private Image ResizeImage(Image img, int width, int height)
        {
            return new Bitmap(img, new Size(width, height));
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Hiển thị hộp thoại chọn file
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Lấy đường dẫn file
                        string imagePath = openFileDialog.FileName;
                        pictureBox1.Image = Image.FromFile(imagePath); // Hiển thị ảnh trong PictureBox

                        // Cập nhật đường dẫn ảnh cho sinh viên đã chọn
                        DataGridViewRow row = dataGridView1.SelectedRows[0];
                        string mssv = row.Cells["mssv"].Value.ToString();

                        // Lưu đường dẫn ảnh vào cơ sở dữ liệu
                        StudentService studentService = new StudentService();
                        studentService.UpdateStudentImage(mssv, imagePath); // Phương thức này cần được định nghĩa trong StudentService

                        // Cập nhật lại DataGridView (nếu cần)
                        LoadData(); // Phương thức này cần được định nghĩa để load lại danh sách sinh viên
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string mssv = txbId.Text.Trim();
            string tenSinhVien = txbName.Text.Trim();
            float.TryParse(txbAverage.Text, out float diemTB);
            int facultyId = (int)cbbFaculty.SelectedValue;

            // Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(tenSinhVien))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

         
            bool isUpdated = studentService.UpdateStudent(mssv, tenSinhVien, facultyId, diemTB);

            if (isUpdated)
            {
                MessageBox.Show("Cập nhật thông tin sinh viên thành công");
                Form1_Load(sender, e); 
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin sinh viên không thành công");
            }
        }

   

        private void btnSign_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Form2"] == null)
            {
                // Tạo một instance của FormRegisterMajor
                Form2 registerMajorForm = new Form2();

                // Hiển thị form đăng ký chuyên ngành
                registerMajorForm.Show();
            }
            else
            {
                // Đưa form đã mở ra trước màn hình
                Application.OpenForms["Form2"].BringToFront();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Kiểm tra xem có hàng nào được chọn không
            {
                // Lấy hàng hiện tại được chọn
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                // Lấy giá trị từ các cột và gán vào TextBox tương ứng
                txbId.Text = row.Cells["mssv"].Value.ToString();
                txbName.Text = row.Cells["tenSinhVien"].Value.ToString();
                txbAverage.Text = row.Cells["diemTB"].Value.ToString();

                // Tìm kiếm và chọn khoa trong ComboBox dựa trên tên khoa
                string facultyName = row.Cells["facultyName"].Value.ToString();
                cbbFaculty.SelectedIndex = cbbFaculty.FindStringExact(facultyName);

                // Lấy đường dẫn ảnh từ cột ImagePath
                string imagePath = row.Cells["ImagePath"].Value?.ToString();

                // Hiển thị ảnh trong PictureBox
                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    pictureBox1.Image = Image.FromFile(imagePath);
                }
                else
                {
                    pictureBox1.Image = null; // Đặt ảnh về null nếu không có đường dẫn hợp lệ
                }
            }
        }



        private void btnView_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);  
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            LoadData(checkBox1.Checked);
        }
    }
}

