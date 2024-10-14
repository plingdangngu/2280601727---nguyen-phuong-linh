namespace DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string mssv { get; set; }

        [StringLength(100)]
        public string tenSinhVien { get; set; }

        public double? diemTB { get; set; }

        public int? facultyId { get; set; }

        public int? majorId { get; set; }

        [StringLength(255)]
        public string imagePath { get; set; }

        public virtual Faculty Faculty { get; set; }

        public virtual Major Major { get; set; }
    }
}
