using OCP.API.Models;
using OCP.Database.Models;

namespace OCP.API.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemUnit> ItemUnits { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderNotification> OrderNotifications { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<PageRole> PageRoles { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<Pincode> Pincodes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<StoreUser> StoreUsers { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("addresses_pkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Addresses)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("addresses_order_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses).HasConstraintName("addresses_user_id_fkey");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_logs_pkey");

            entity.Property(e => e.ActivityDate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("carts_pkey");

            entity.Property(e => e.TotalItems).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("carts_user_id_fkey");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cart_details_pkey");

            entity.Property(e => e.ItemQuantity).HasDefaultValue(0);

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails).HasConstraintName("cart_details_cart_id_fkey");

            entity.HasOne(d => d.Item).WithMany(p => p.CartDetails).HasConstraintName("cart_details_item_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.CartDetails).HasConstraintName("cart_details_unit_id_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("discounts_pkey");

            entity.Property(e => e.IsApplicable).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Store).WithMany(p => p.Discounts).HasConstraintName("discounts_store_id_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("items_pkey");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("now()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Items)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("items_sub_category_id_fkey");
        });

        modelBuilder.Entity<ItemUnit>(entity =>
        {
            entity.HasKey(e => new { e.ItemId, e.UnitId, e.StoreId }).HasName("item_units_pkey");

            entity.Property(e => e.IsExistsInOrder).HasDefaultValue(false);

            entity.HasOne(d => d.Item).WithMany(p => p.ItemUnits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_units_item_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.ItemUnits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_units_store_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.ItemUnits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_units_unit_id_fkey");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("menu_pkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OrderDate).HasDefaultValueSql("now()");
            entity.Property(e => e.OverallDiscountPer).HasDefaultValueSql("0");
            entity.Property(e => e.TotalPrice).HasDefaultValueSql("0");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders).HasConstraintName("orders_store_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_details_pkey");

            entity.Property(e => e.ActualRate).HasDefaultValueSql("0");
            entity.Property(e => e.DiscountPer).HasDefaultValueSql("0");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ItemQuantity).HasDefaultValueSql("0");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderDetails).HasConstraintName("order_details_item_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasConstraintName("order_details_order_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.OrderDetails).HasConstraintName("order_details_store_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.OrderDetails).HasConstraintName("order_details_unit_id_fkey");
        });

        modelBuilder.Entity<OrderNotification>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.UserId }).HasName("order_notifications_pkey");

            entity.Property(e => e.IsNotify).HasDefaultValue(false);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_notifications_order_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.OrderNotifications)
                .HasConstraintName("order_notifications_store_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.OrderNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_notifications_user_id_fkey");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pages_pkey");

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Menu).WithMany(p => p.Pages).HasConstraintName("pages_menu_id_fkey");
        });

        modelBuilder.Entity<PageRole>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PageId }).HasName("page_roles_pkey");

            entity.Property(e => e.IsReadable).HasDefaultValue(false);
            entity.Property(e => e.IsVisible).HasDefaultValue(false);
            entity.Property(e => e.IsWritable).HasDefaultValue(false);

            entity.HasOne(d => d.Page).WithMany(p => p.PageRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("page_roles_page_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.PageRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("page_roles_role_id_fkey");
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => new { e.PhoneNo, e.Otp }).HasName("phones_pkey");

            entity.Property(e => e.IsVerified).HasDefaultValue(false);

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.Phones)
                .HasConstraintName("fk_phones_application_user");
        });

        modelBuilder.Entity<Pincode>(entity =>
        {
            entity.HasKey(e => new { e.Pin, e.StoreId }).HasName("pincodes_pkey");

            entity.HasOne(d => d.Store).WithMany(p => p.Pincodes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pincodes_store_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => new { e.StatusId, e.StatusType }).HasName("status_pkey");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stocks_pkey");

            entity.Property(e => e.DiscountPer).HasDefaultValueSql("0");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.IsDiscountApplicable).HasDefaultValue(false);
            entity.Property(e => e.ItemQuantity).HasDefaultValueSql("0");

            entity.HasOne(d => d.InitialUnit).WithMany(p => p.Stocks).HasConstraintName("stocks_initial_unit_id_fkey");

            entity.HasOne(d => d.Item).WithMany(p => p.Stocks).HasConstraintName("stocks_item_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.Stocks).HasConstraintName("stocks_store_id_fkey");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Stocks).HasConstraintName("stocks_sub_category_id_fkey");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stores_pkey");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<StoreUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("store_users_pkey");

            entity.HasOne(d => d.Store).WithMany(p => p.StoreUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("store_users_store_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.StoreUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("store_users_user_id_fkey");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sub_categories_pkey");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sub_categories_category_id_fkey");

            entity.HasOne(d => d.Store).WithMany(p => p.SubCategories)
                .HasConstraintName("sub_categories_store_id_fkey");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("units_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.Property(e => e.CountryCode)
                .HasDefaultValueSql("'IN'::bpchar")
                .IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
            entity.Property(e => e.IsConfirmed).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Uuid).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_roles_pkey");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_roles_role_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasConstraintName("user_roles_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
