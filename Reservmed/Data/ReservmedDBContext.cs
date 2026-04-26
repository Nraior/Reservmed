using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reservmed.Models.Domain;
using Reservmed.Models.Identity;

namespace Reservmed.Data
{
    public class ReservmedDBContext : IdentityDbContext<ApplicationUser>
    {
        public ReservmedDBContext(DbContextOptions<ReservmedDBContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Doctor>()
                .HasMany(d => d.Specializations)
                .WithMany(s => s.Doctors)
                .UsingEntity<DoctorSpecialization>(
                    j =>
                    {
                        j.HasKey(ds => new { ds.SpecializationId, ds.DoctorId });
                        j.ToTable("DoctorSpecializations");
                    }
                );

            builder.Entity<Specialization>()
                .HasData(
                new Specialization { Id = 1, Name = "Cardiology" },
                new Specialization { Id = 2, Name = "Dermatologist" },
                new Specialization { Id = 3, Name = "Internist" }
            );

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d",
                    Name = Common.UserRoles.Doctor,
                    NormalizedName = Common.UserRoles.Doctor.ToUpper()
                },
                new IdentityRole
                {
                    Id = "f6e5d4c3-b2a1-0d9c-8b7a-6c5b4a3f2e1d",
                    Name = Common.UserRoles.Patient,
                    NormalizedName = Common.UserRoles.Patient.ToUpper()
                }
                );

        }

    }
}
