using FintranetTest.Domain.Aggregates;
using FintranetTest.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FintranetTest.Persistence.Mappings;

internal class CustomerMapping : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.Firstname)
            .HasColumnName(nameof(Customer.Firstname))
            .HasMaxLength(Name.MaxLenght)
            .IsUnicode(false)
            .IsRequired()
            .HasConversion(c => c.Value, c => Name.Create(c).Value);

        builder.Property(c => c.Lastname)
            .HasColumnName(nameof(Customer.Lastname))
            .HasMaxLength(Name.MaxLenght)
            .IsUnicode(false)
            .IsRequired()
            .HasConversion(c => c.Value, c => Name.Create(c).Value);

        builder.Property(c => c.PhoneNumber)
            .HasColumnName(nameof(Customer.PhoneNumber))
            .HasMaxLength(PhoneNumber.FixedLenght)
            .IsUnicode(false)
            .IsRequired()
            .HasConversion(c => c.Value, c => PhoneNumber.Create(c).Value);

        builder.Property(c => c.Email)
            .HasColumnName(nameof(Customer.Email))
            .HasMaxLength(Email.MaxLenght)
            .IsUnicode(false)
            .IsRequired()
            .HasConversion(c => c.Value, c => Email.Create(c).Value);

        builder.Property(c => c.BankAccountNumber)
            .HasColumnName(nameof(Customer.BankAccountNumber))
            .HasMaxLength(BankAccountNumber.MaxLenght)
            .IsUnicode(false)
            .IsRequired()
            .HasConversion(c => c.Value, c => BankAccountNumber.Create(c).Value);

        builder.Property(c => c.DateOfBirth)
            .HasColumnName(nameof(Customer.DateOfBirth))
            .IsRequired()
            .HasConversion(toProvider => toProvider.ToDateTime(TimeOnly.MinValue),
                  fromProvider => DateOnly.FromDateTime(fromProvider))
            .HasColumnType("DATE");

        builder.HasIndex(c => c.Email)
            .IsUnique(true);

        builder.HasIndex(c => new { c.Firstname, c.Lastname, c.DateOfBirth })
            .IsUnique(true);
    }
}
