using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class MajorService
    {
        private readonly MajorRepository majorRepository; // Giả sử bạn đã tạo một lớp Repository cho Major

        public MajorService()
        {
            majorRepository = new MajorRepository();
        }


        public List<Major> GetMajorByFaculty(int facultyId)
        {
            // Kiểm tra ID khoa hợp lệ
            if (facultyId <= 0)
            {
                throw new ArgumentException("ID khoa không hợp lệ.");
            }

           
            return majorRepository.GetMajorsByFacultyId(facultyId);
        }
    }
    public class MajorRepository
    {
        private readonly Model1 db; // Giả sử Model1 là lớp DbContext của bạn

        public MajorRepository()
        {
            db = new Model1();
        }

        // Phương thức lấy chuyên ngành theo ID khoa
        public List<Major> GetMajorsByFacultyId(int facultyId)
        {
            return db.Majors
                .Where(m => m.facultyId == facultyId) // Giả sử bạn có cột FacultyId trong bảng Major
                .ToList();
        }
    }

}
