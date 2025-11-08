-- Connect to the target database named 'ocp_db'.
-- This is a standard psql command to switch the current database connection.
\c ocp_db;

-- -------------------------------------------------------------------------
-- Schema Creation
-- -------------------------------------------------------------------------

-- Create schemas (logical containers) for different functional areas of the application.
-- 'CREATE SCHEMA IF NOT EXISTS' prevents an error if the schema already exists.

CREATE SCHEMA IF NOT EXISTS users; -- Schema for user accounts, profiles, and authentication-related data (if not using 'security').
CREATE SCHEMA IF NOT EXISTS security; -- Schema for roles, permissions, access control lists (ACLs), and security configuration.
CREATE SCHEMA IF NOT EXISTS audit; -- Schema dedicated to logging all significant changes and user actions for auditing purposes.
CREATE SCHEMA IF NOT EXISTS navigation; -- Schema for site structure, menus, links, and possibly breadcrumbs data.
CREATE SCHEMA IF NOT EXISTS store; -- Schema for managing store-specific information like hours, locations, and settings.
CREATE SCHEMA IF NOT EXISTS catalog; -- Schema for product data, categories, specifications, and inventory.
CREATE SCHEMA IF NOT EXISTS pricing; -- Schema for prices, discounts, promotions, and tax rates.
CREATE SCHEMA IF NOT EXISTS cart; -- Schema for managing temporary shopping cart and wishlist data.
CREATE SCHEMA IF NOT EXISTS orders; -- Schema for handling all order-related transactions and history.
CREATE SCHEMA IF NOT EXISTS comm; -- Schema for communication-related data (e.g., email templates, notifications, messaging).
CREATE SCHEMA IF NOT EXISTS geo; -- Schema for geographical data like countries, regions, cities, and postal codes.
CREATE SCHEMA IF NOT EXISTS meta;
-- Schema for application metadata, configuration settings, and lookup tables.

-- -------------------------------------------------------------------------
-- Schema Ownership Assignment
-- -------------------------------------------------------------------------

-- Change the owner of each newly created schema to the specified user 'ocp_user'.
-- This is a best practice for security and privilege management, ensuring the application
-- user (ocp_user) has full control over the application's schemas and objects within.

ALTER
SCHEMA security OWNER TO ocp_user;
ALTER
SCHEMA users OWNER TO ocp_user;
ALTER
SCHEMA audit OWNER TO ocp_user;
ALTER
SCHEMA navigation OWNER TO ocp_user;
ALTER
SCHEMA store OWNER TO ocp_user;
ALTER
SCHEMA catalog OWNER TO ocp_user;
ALTER
SCHEMA pricing OWNER TO ocp_user;
ALTER
SCHEMA cart OWNER TO ocp_user;
ALTER
SCHEMA orders OWNER TO ocp_user;
ALTER
SCHEMA comm OWNER TO ocp_user;
ALTER
SCHEMA geo OWNER TO ocp_user;
ALTER
SCHEMA meta OWNER TO ocp_user;