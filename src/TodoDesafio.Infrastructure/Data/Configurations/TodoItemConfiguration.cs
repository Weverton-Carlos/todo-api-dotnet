using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Infrastructure.Data.Configurations;

public class TodoItemConfiguration: IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("todo_items");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        // salva como int no banco
        builder.Property(t => t.Status)
            .HasConversion<int>()
            .HasDefaultValue(Status.Pending);


        builder.Property(t => t.DueDate)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.UpdatedAt)
            .IsRequired(false);
        
        builder.Property(t => t.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasQueryFilter(t => !t.IsDeleted);
 
        builder.HasIndex(t => new { t.Status, t.DueDate });
        builder.HasIndex(t => t.IsDeleted);
    }
}