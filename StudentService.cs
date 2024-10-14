using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class StudentService
    {
        public List<Student> GetALL()
        {
            Model1 db = new Model1();
            return db.Students.ToList();
        }

        public bool AddStudent(string mssv, string tenSinhVien, int facultyId, float diemTB)
        {
            Model1 db = new Model1();


            try
            {
                var exisStudent = db.Students.FirstOrDefault(s => s.mssv == mssv);
                if (exisStudent != null)
                {
                    return false;
                }

                var student = new Student
                {
                    mssv = mssv,
                    tenSinhVien = tenSinhVien,
                    diemTB = diemTB,
                    facultyId = facultyId

                };

                db.Students.Add(student);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Student> GetAllStudentsWithNoMajor()
        {
            using (var db = new Model1())
            {
                return db.Students.Where(s => s.Major.majorName == null).ToList();
            }
        }


        public List<Faculty> GetAllFaculties()
        {
            using (var db = new Model1())
            {
                return db.Faculties.ToList();
            }
        }



        public void DeleteStudent(string mssv)
        {
            using (var db = new Model1())
            {

                var student = db.Students.FirstOrDefault(s => s.mssv == mssv);

                if (student == null)
                {
                    throw new Exception("Sinh viên không tồn tại.");
                }


                db.Students.Remove(student);
                db.SaveChanges();
            }
        }

        public bool UpdateStudent(string mssv, string tenSinhVien, int facultyId, float diemTB)
        {
            try
            {
                using (var db = new Model1())
                {
                    
                    var student = db.Students.FirstOrDefault(s => s.mssv == mssv);

                    if (student != null)
                    {
                       
                        student.tenSinhVien = tenSinhVien;
                        student.facultyId = facultyId;
                        student.diemTB = diemTB;

                        
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (nếu có)
                Console.WriteLine("Lỗi khi cập nhật sinh viên: " + ex.Message);
                return false;
            }
        }


        public void UpdateStudentImage(string mssv, string imagePath)
        {
            using (var context = new Model1())
            {
                var student = context.Students.FirstOrDefault(s => s.mssv == mssv);
                if (student != null)
                {
                    student.imagePath = imagePath; // Cập nhật đường dẫn ảnh
                    context.SaveChanges(); // Lưu thay đổi
                }
            }
        }


        public bool UpdateStudentMajor(string mssv, int facultyId, int majorId)
        {
            using (var context = new Model1())
            {
                // Tìm sinh viên theo MSSV
                var student = context.Students.FirstOrDefault(s => s.mssv == mssv);
                if (student != null)
                {
                    // Cập nhật khoa và chuyên ngành cho sinh viên
                    student.facultyId = facultyId;
                    student.majorId = majorId;

                    // Lưu thay đổi vào database
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }


    }
}
