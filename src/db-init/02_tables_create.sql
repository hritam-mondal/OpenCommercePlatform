-- ---------------------------------------------------------------------
-- USERS SCHEMA: Core User Account Management
-- ---------------------------------------------------------------------
-- USERS TABLE: Stores the main details for all application users.
CREATE TABLE users.users (
    -- Primary Key and Identification
    user_id bigserial NOT NULL,
    -- System-generated unique ID (Primary Key).
    "uuid" uuid DEFAULT gen_random_uuid() NOT NULL,
    -- Publicly exposed, non-sequential unique identifier.
    username varchar(50) NOT NULL,
    -- Unique identifier for login.
    -- Personal Information
    first_name varchar(50) NOT NULL,
    last_name varchar(50) NOT NULL,
    email varchar(254) NOT NULL,
    -- Unique email address.
    phone_number varchar(25) NULL,
    -- Optional phone number.
    country_code bpchar(2) DEFAULT 'IN' :: bpchar NULL,
    -- Standard 2-character country code (e.g., 'IN').
    -- Security and Authentication
    password_hash text NOT NULL,
    -- Stores the secure hash of the password.
    password_salt text NULL,
    -- Optional salt if not included in the hash.
    password_reset_token uuid NULL,
    -- Token for resetting password.
    reset_token_expires_at timestamptz NULL,
    -- Expiry time for the password reset token.
    -- Status and Roles
    is_active bool DEFAULT true NOT NULL,
    -- Can the user log in?
    is_admin bool DEFAULT false NOT NULL,
    -- Quick check for administrative privileges.
    is_confirmed bool DEFAULT false NOT NULL,
    -- Has the user confirmed their email?
    email_confirm_token uuid NULL,
    -- Token for email confirmation.
    email_confirm_token_expires timestamptz NULL,
    -- Expiry time for the email confirmation token.
    -- Auditing Timestamps
    created_at timestamptz DEFAULT CURRENT_TIMESTAMP NOT NULL,
    -- Record creation timestamp.
    updated_at timestamptz DEFAULT CURRENT_TIMESTAMP NOT NULL,
    -- Last record update timestamp.
    last_login_at timestamptz NULL,
    -- Last successful login timestamp.
    -- Constraints
    CONSTRAINT users_email_check CHECK (
        (
            (email) :: text ~ '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$' :: text -- Basic email format validation.
        )
    ),
    CONSTRAINT users_email_key UNIQUE (email),
    CONSTRAINT users_pkey PRIMARY KEY (user_id),
    CONSTRAINT users_username_check CHECK (
        (
            (username) :: text ~ '^[a-zA-Z0-9_]{3,50}$' :: text
        )
    ),
    -- Username format validation (alphanumeric and underscore).
    CONSTRAINT users_username_key UNIQUE (username),
    CONSTRAINT users_uuid_key UNIQUE (uuid)
);

---
-- ---------------------------------------------------------------------
-- SECURITY SCHEMA: Roles and Permissions
-- ---------------------------------------------------------------------
-- Roles master: Defines all available user roles (e.g., 'Customer', 'Manager', 'Admin').
CREATE TABLE IF NOT EXISTS security.roles (
    id serial PRIMARY KEY,
    name varchar (50) NOT NULL UNIQUE -- The name of the role.
);

-- Mapping between application users and roles (Many-to-Many relationship).
CREATE TABLE IF NOT EXISTS security.user_roles (
    id serial PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES users.users (user_id) ON
    DELETE
        CASCADE,
        -- Link to the user, cascade delete if user is removed.
        role_id int NOT NULL REFERENCES security.roles (id) -- Link to the role.
);

CREATE INDEX IF NOT EXISTS idx_user_roles_user ON security.user_roles (user_id);

-- Index for fast lookup of roles by user.
CREATE INDEX IF NOT EXISTS idx_user_roles_role ON security.user_roles (role_id);

-- Index for fast lookup of users by role.
---
-- ---------------------------------------------------------------------
-- AUDIT SCHEMA: Tracking user activities and system events
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS audit.audit_logs (
    id serial PRIMARY KEY,
    ref_id varchar (100),
    -- ID of the object/record being audited (e.g., an order_id).
    user_identifier varchar (50),
    -- Username or user_id of the user who performed the activity.
    activity text,
    -- Description of the action performed (e.g., "Updated item X").
    activity_date timestamptz NOT NULL DEFAULT now () -- When the activity occurred.
);

CREATE INDEX IF NOT EXISTS idx_audit_logs_ref_id ON audit.audit_logs (ref_id);

CREATE INDEX IF NOT EXISTS idx_audit_logs_user_identifier ON audit.audit_logs (user_identifier);

---
-- ---------------------------------------------------------------------
-- NAVIGATION SCHEMA: Site structure and Role-Based Access Control (RBAC) for pages
-- ---------------------------------------------------------------------
-- Top-level menu entries (e.g., 'Catalog', 'Orders', 'Reports').
CREATE TABLE IF NOT EXISTS navigation.menu (
    id serial PRIMARY KEY,
    display_name varchar (100) NOT NULL
);

-- Pages under menus (e.g., 'View Orders' under 'Orders').
CREATE TABLE IF NOT EXISTS navigation.pages (
    id serial PRIMARY KEY,
    menu_id int REFERENCES navigation.menu (id),
    -- Foreign key to the parent menu.
    display_name varchar (200),
    is_deleted boolean NOT NULL DEFAULT false -- Soft delete flag.
);

-- Role-based page permissions: Controls which roles can see and interact with which pages.
CREATE TABLE IF NOT EXISTS navigation.page_roles (
    role_id int NOT NULL REFERENCES security.roles (id),
    -- Role being granted permission.
    page_id int NOT NULL REFERENCES navigation.pages (id),
    -- Page the permission applies to.
    is_visible boolean NOT NULL DEFAULT false,
    -- Can the role see this page/menu item?
    is_readable boolean NOT NULL DEFAULT false,
    -- Can the role view data on this page?
    is_writable boolean NOT NULL DEFAULT false,
    -- Can the role modify data on this page?
    PRIMARY KEY (role_id, page_id) -- Composite key ensures one entry per role/page pair.
);

---
-- ---------------------------------------------------------------------
-- STORE SCHEMA: Store/Branch Management
-- ---------------------------------------------------------------------
-- Physical/virtual stores: Defines individual business units or branches.
CREATE TABLE IF NOT EXISTS store.stores (
    id serial PRIMARY KEY,
    name varchar (200),
    is_active boolean NOT NULL DEFAULT true,
    image_url varchar (500),
    company_name varchar (300),
    company_address text,
    gstn varchar (50) -- Goods and Services Tax Network (for India, or similar tax ID).
);

-- Map users to stores (e.g., a manager is assigned to specific stores).
CREATE TABLE IF NOT EXISTS store.store_users (
    id serial PRIMARY KEY,
    user_id bigint NOT NULL REFERENCES users.users (user_id),
    store_id int NOT NULL REFERENCES store.stores (id)
);

CREATE INDEX IF NOT EXISTS idx_store_users_user ON store.store_users (user_id);

CREATE INDEX IF NOT EXISTS idx_store_users_store ON store.store_users (store_id);

---
-- ---------------------------------------------------------------------
-- CATALOG SCHEMA: Product/Item and Stock Management
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS catalog.categories (
    id serial PRIMARY KEY,
    name varchar (100) NOT NULL,
    is_active boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS catalog.sub_categories (
    id serial PRIMARY KEY,
    name varchar (100) NOT NULL,
    category_id int NOT NULL REFERENCES catalog.categories (id),
    -- Link to the parent category.
    is_active boolean NOT NULL DEFAULT true,
    store_id int NULL REFERENCES store.stores (id) -- Optional link for store-specific subcategories.
);

-- Defines units of measure (e.g., 'kg', 'Liter', 'Piece').
CREATE TABLE IF NOT EXISTS catalog.units (
    id serial PRIMARY KEY,
    unit_name varchar (100),
    unit_order int,
    -- Used for sorting units.
    conversion_ratio numeric NULL -- Ratio to convert to a base unit (e.g., 1000 for kg to gram).
);

-- Main table for product definition.
CREATE TABLE IF NOT EXISTS catalog.items (
    id serial PRIMARY KEY,
    sub_category_id int NOT NULL REFERENCES catalog.sub_categories (id),
    name varchar (200) NOT NULL,
    description varchar (1000),
    short_description varchar (300),
    image_url varchar (500),
    created_on timestamptz NOT NULL DEFAULT now (),
    created_by varchar (100),
    is_active boolean NOT NULL DEFAULT true
);

-- Item availability per unit and store: Defines which unit is available for a product at a specific store.
CREATE TABLE IF NOT EXISTS catalog.item_units (
    item_id int NOT NULL REFERENCES catalog.items (id),
    unit_id int NOT NULL REFERENCES catalog.units (id),
    store_id int NOT NULL REFERENCES store.stores (id),
    is_exists_in_order boolean NOT NULL DEFAULT false,
    -- Indicates if this item/unit combo is sellable/orderable.
    PRIMARY KEY (item_id, unit_id, store_id)
);

-- Stock entries: Represents current inventory levels and pricing per item/store.
CREATE TABLE IF NOT EXISTS catalog.stocks (
    id serial PRIMARY KEY,
    item_id int REFERENCES catalog.items (id),
    item_quantity numeric (18, 3) DEFAULT 0,
    -- Current quantity in stock.
    rate numeric (18, 2) NOT NULL DEFAULT 0,
    -- Selling rate/price per unit.
    rate_for varchar (200),
    -- Description of what the rate applies to.
    initial_unit_id int REFERENCES catalog.units (id),
    -- The unit of the stock quantity.
    store_id int REFERENCES store.stores (id),
    is_active boolean NOT NULL DEFAULT true,
    is_available boolean NOT NULL DEFAULT true,
    reason varchar (500),
    -- Reason for non-availability/status change.
    sub_category_id int REFERENCES catalog.sub_categories (id),
    -- Redundant FK for faster lookups/partitioning.
    discount_remarks varchar (500),
    discount_per numeric (10, 2) DEFAULT 0,
    is_discount_applicable boolean NOT NULL DEFAULT false
);

CREATE INDEX IF NOT EXISTS idx_stocks_item ON catalog.stocks (item_id);

CREATE INDEX IF NOT EXISTS idx_stocks_store ON catalog.stocks (store_id);

---
-- ---------------------------------------------------------------------
-- PRICING SCHEMA: Discount Management
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS pricing.discounts (
    id serial PRIMARY KEY,
    discount_per numeric (10, 2),
    -- Percentage discount.
    discount_amt numeric (18, 2),
    -- Fixed amount discount.
    is_applicable boolean DEFAULT false,
    is_deleted boolean DEFAULT false,
    remarks varchar (500),
    store_id int REFERENCES store.stores (id) -- Link to store for store-specific discounts.
);

---
-- ---------------------------------------------------------------------
-- CART SCHEMA: Shopping Cart Management
-- ---------------------------------------------------------------------
-- Carts header: Represents a user's current shopping cart.
CREATE TABLE IF NOT EXISTS cart.carts (
    id serial PRIMARY KEY,
    user_id bigint REFERENCES users.users (user_id),
    -- Link to the user who owns the cart.
    total_items int DEFAULT 0 -- Count of distinct items in the cart.
);

-- Cart line items: Details of the products added to a specific cart.
CREATE TABLE IF NOT EXISTS cart.cart_details (
    id serial PRIMARY KEY,
    cart_id int NOT NULL REFERENCES cart.carts (id) ON
    DELETE
        CASCADE,
        -- Link to cart header, delete details when cart is deleted.
        item_id int REFERENCES catalog.items (id),
        item_quantity int DEFAULT 0,
        rate numeric (18, 2),
        -- Rate per unit at the time of addition.
        item_price numeric (18, 2),
        -- Total price for this line item.
        unit_id int REFERENCES catalog.units (id)
);

CREATE INDEX IF NOT EXISTS idx_cart_details_cart ON cart.cart_details (cart_id);

---
-- ---------------------------------------------------------------------
-- ORDERS SCHEMA: Order Processing and History
-- ---------------------------------------------------------------------
-- Orders header: Main transaction record for an order.
CREATE TABLE IF NOT EXISTS orders.orders (
    order_id varchar (50) PRIMARY KEY,
    order_name varchar (200),
    order_date timestamptz DEFAULT now (),
    total_price numeric (18, 2) DEFAULT 0,
    -- Price before discounts/final calculations.
    user_id bigint NOT NULL REFERENCES users.users (user_id),
    order_note varchar (500),
    order_status varchar (50),
    -- e.g., 'Pending', 'Processing', 'Delivered'.
    payment_status varchar (50),
    -- e.g., 'Paid', 'Unpaid', 'Refunded'.
    is_active boolean NOT NULL DEFAULT true,
    bank_tran_id varchar (200),
    -- Bank transaction ID.
    remarks text,
    payment_mode varchar (50),
    -- e.g., 'Credit Card', 'COD'.
    gateway_tran_id varchar (200),
    -- Payment gateway transaction ID.
    bank_name varchar (200),
    tran_status varchar (200),
    -- Transaction status from the gateway.
    store_id int REFERENCES store.stores (id),
    final_price numeric (18, 2),
    -- The final price paid by the customer.
    overall_discount_per numeric (10, 2) DEFAULT 0
);

CREATE INDEX IF NOT EXISTS idx_orders_user ON orders.orders (user_id);

CREATE INDEX IF NOT EXISTS idx_orders_store ON orders.orders (store_id);

-- Order line items: Details of products included in an order.
CREATE TABLE IF NOT EXISTS orders.order_details (
    id serial PRIMARY KEY,
    order_id varchar (50) NOT NULL REFERENCES orders.orders (order_id) ON
    DELETE
        CASCADE,
        -- Link to order header, delete details when order is cancelled/deleted.
        item_id int REFERENCES catalog.items (id),
        item_name varchar (300),
        item_quantity numeric (18, 3) DEFAULT 0,
        unit_id int REFERENCES catalog.units (id),
        rate numeric (18, 2),
        item_price numeric (18, 2),
        is_deleted boolean DEFAULT false,
        store_id int REFERENCES store.stores (id),
        discount_per numeric (10, 2) DEFAULT 0,
        actual_rate numeric (18, 2) DEFAULT 0 -- Rate after discount.
);

CREATE INDEX IF NOT EXISTS idx_order_details_order ON orders.order_details (order_id);

-- Notification tracking for orders per user.
CREATE TABLE IF NOT EXISTS orders.order_notifications (
    order_id varchar (50) NOT NULL REFERENCES orders.orders (order_id),
    user_id bigint NOT NULL REFERENCES users.users (user_id),
    store_id int REFERENCES store.stores (id),
    is_notify boolean DEFAULT false,
    -- Has the user been notified about this order change?
    PRIMARY KEY (order_id, user_id)
);

-- Addresses for orders and/or users (Billing/Shipping).
CREATE TABLE IF NOT EXISTS orders.addresses (
    id serial PRIMARY KEY,
    order_id varchar (50) REFERENCES orders.orders (order_id) ON
    DELETE
    SET
        NULL,
        -- Address used for a specific order.
        user_id bigint REFERENCES users.users (user_id),
        -- Saved address for a user.
        full_name varchar (200),
        address text,
        city varchar (200),
        pin varchar (50),
        phone varchar (50),
        email varchar (200),
        address_type varchar (50),
        -- e.g., 'Shipping', 'Billing', 'Home'.
        state varchar (200)
);

---
-- ---------------------------------------------------------------------
-- GEO SCHEMA: Geographical Data
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS geo.pincodes (
    pin varchar (50) NOT NULL,
    deliver_day varchar (50),
    -- Estimated delivery time/day for this pincode.
    store_id int NOT NULL REFERENCES store.stores (id),
    PRIMARY KEY (pin, store_id) -- A pincode's delivery status may be store-specific.
);

---
-- ---------------------------------------------------------------------
-- COMM SCHEMA: Communications (e.g., OTP for phone verification)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS comm.phones (
    phone_no varchar(50) NOT NULL,
    otp varchar(50) NOT NULL,
    -- One-Time Password sent.
    expire_date timestamptz,
    is_verified boolean DEFAULT false,
    application_user_id bigint,
    -- use bigint to match users.users(user_id) BIGSERIAL
    PRIMARY KEY (phone_no, otp),
    CONSTRAINT fk_phones_application_user FOREIGN KEY (application_user_id) REFERENCES users.users (user_id) -- optional: ON DELETE SET NULL or ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_phones_user ON comm.phones (application_user_id);

---
-- ---------------------------------------------------------------------
-- META SCHEMA: Application Metadata and Configuration
-- ---------------------------------------------------------------------
-- Generic table for status codes/types used across the application.
CREATE TABLE IF NOT EXISTS meta.status (
    status_id varchar (50) NOT NULL,
    -- The specific status code (e.g., 'ORD_PENDING').
    status_type varchar (50) NOT NULL,
    -- The category of the status (e.g., 'ORDER_STATUS', 'PAYMENT_STATUS').
    status_text varchar (200),
    -- The display name.
    description text,
    PRIMARY KEY (status_id, status_type)
);

---
-- ---------------------------------------------------------------------
-- Ownership & Grants
-- ---------------------------------------------------------------------
-- PL/pgSQL block to dynamically loop through all newly created tables
-- and set their owner to 'ocp_user' and grant all privileges.
DO $$
DECLARE
    r record;

BEGIN
    FOR r IN (
        SELECT
            schemaname,
            tablename
        FROM
            pg_tables
        WHERE
            schemaname IN (
                'users',
                'security',
                'audit',
                'navigation',
                'store',
                'catalog',
                'pricing',
                'cart',
                'orders',
                'comm',
                'geo',
                'meta'
            )
    )
    LOOP
        -- Alter ownership of the table to ocp_user.
        EXECUTE format(
            'ALTER TABLE %I.%I OWNER TO ocp_user',
            r.schemaname,
            r.tablename
        );

-- Grant all permissions on the table to ocp_user (redundant if ocp_user is owner, but good practice).
EXECUTE format(
    'GRANT ALL ON TABLE %I.%I TO ocp_user',
    r.schemaname,
    r.tablename
);

END
LOOP
;

END $$;
