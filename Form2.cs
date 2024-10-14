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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace buoi_5
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy MSSV của sinh viên đã chọn từ DataGridView
                string mssv = dataGridView1.SelectedRows[0].Cells["mssv"].Value.ToString();

                // Kiểm tra xem đã chọn Khoa và Chuyên ngành chưa
                if (cbbFaculty.SelectedValue != null && cbbMajor.SelectedValue != null)
                {
                    int facultyId = int.Parse(cbbFaculty.SelectedValue.ToString());
                    int majorId = int.Parse(cbbMajor.SelectedValue.ToString());

                    // Tạo một đối tượng StudentService để xử lý logic cập nhật
                    StudentService studentService = new StudentService();

                    // Cập nhật thông tin sinh viên với khoa và chuyên ngành đã chọn
                    bool isUpdated = studentService.UpdateStudentMajor(mssv, facultyId, majorId);

                    if (isUpdated)
                    {
                        MessageBox.Show("Đăng ký chuyên ngành thành công!");
                        // Bạn có thể cập nhật lại DataGridView hoặc làm gì đó sau khi cập nhật thành công
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi đăng ký chuyên ngành.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn khoa và chuyên ngành.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ danh sách.");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            FacultyService facultyService = new FacultyService();
            List<Faculty> faculties = facultyService.GetAll();

            // Thiết lập DataSource cho cbbFaculty
            cbbFaculty.DataSource = faculties;
            cbbFaculty.DisplayMember = "facultyName";
            cbbFaculty.ValueMember = "facultyId";

            // Gọi sự kiện SelectedIndexChanged một lần để cập nhật majors
            cbbFaculty.SelectedIndexChanged += cbbFaculty_SelectedIndexChanged;

            // Chọn khoa mặc định
            var defaultFaculty = faculties.FirstOrDefault(f => f.facultyName == "Công Nghệ Thông Tin");
            if (defaultFaculty != null)
            {
                cbbFaculty.SelectedItem = defaultFaculty;
            }

            // Kiểm tra và cập nhật majors
            cbbFaculty_SelectedIndexChanged(cbbFaculty, EventArgs.Empty);

            // Thiết lập danh sách sinh viên
            StudentService studentService = new StudentService();
            List<Student> studentsWithNoMajor = studentService.GetAllStudentsWithNoMajor();

            dataGridView1.DataSource = studentsWithNoMajor.Select(s => new
            {
                mssv = s.mssv,
                tenSinhVien = s.tenSinhVien,
                diemTB = s.diemTB,
            }).ToList();

            dataGridView1.Columns["mssv"].HeaderText = "MSSV";
            dataGridView1.Columns["tenSinhVien"].HeaderText = "Tên Sinh Viên";
            dataGridView1.Columns["diemTB"].HeaderText = "Điểm TB";
        }


        private void cbbFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbFaculty.SelectedValue != null)
            {
                if (int.TryParse(cbbFaculty.SelectedValue.ToString(), out int facultyId))
                {
                    MajorService majorService = new MajorService();
                    List<Major> majors = majorService.GetMajorByFaculty(facultyId);

                    cbbMajor.DataSource = majors;
                    cbbMajor.DisplayMember = "majorName";
                    cbbMajor.ValueMember = "majorId";
                }
            }
        }
    }
}
