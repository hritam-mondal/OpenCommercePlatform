INSERT INTO "users".users ("uuid", username, first_name, last_name, email, phone_number, country_code, password_hash,
                           password_salt, password_reset_token, reset_token_expires_at, is_active, is_admin,
                           is_confirmed,
                           email_confirm_token, email_confirm_token_expires, created_at, updated_at, last_login_at)
VALUES ('596d549c-6c7e-40fc-8f35-bf3d46138f9e'::uuid,
        'JLawNce42',
        'Jennifer',
        'Lawrence',
        'jennifer.lawrence@email.com',
        '555-0101',
        'IN',
        'zY7o6Mveuvt3t3dahx/wtcYE1EONNs9nBvplfTfrlEI=',
        'wYISIRm5P/fVpr3p6Sbjfw==',
        NULL,
        NULL,
        true,
        false,
        true,
        '1d0c1f07-668d-45ba-b386-32073c89a4bb'::uuid,
        '2025-10-18 10:46:15.850972+05:30',
        '2025-10-17 10:46:15.886018+05:30',
        '2025-10-17 10:46:15.886018+05:30',
        NULL),
       ('c70cb05c-c2f9-4676-84ac-b13443086ef6'::uuid,
        'Zendaya_Admin',
        'Zendaya',
        'Coleman',
        'zendaya.admin@example.net',
        '555-0165',
        'IN',
        'mXEphjFRjgj5dg9mzcnTQk3pYPd6nfp19eVsBnOKH7o=',
        'WVTFev5mtib5McsH6XWH8g==',
        NULL,
        NULL,
        true,
        true,
        true,
        'fa449d9f-e0e0-4560-9a3f-46d87941f916'::uuid,
        '2025-10-18 10:52:40.736802+05:30',
        '2025-10-17 10:52:40.989696+05:30', '2025-10-17 10:52:40.989696+05:30',
        NULL);

INSERT INTO "store".stores
    ("name", is_active, image_url, company_name, company_address, gstn)
VALUES ('Electronics', true,
        'https://example.com/images/electronics.jpg',
        'TechWorld Pvt. Ltd.',
        '12, Park Street, Kolkata, West Bengal, India - 700016',
        '19AACCT1234L1Z8'),
       ('HomeStyle', true,
        'https://example.com/images/homestyle-store.jpg',
        'HomeStyle Retail India Ltd.',
        'Plot No. 44, HSR Layout, Bengaluru, Karnataka, India - 560102',
        '29AABCH5678N1Z2');

INSERT INTO "catalog".categories ("name", "is_active")
VALUES ('Electronics', true),
       ('Fashion', true);

INSERT INTO "catalog".sub_categories (category_id, "name", "is_active", "store_id")
VALUES (1, 'Smartphones', true, 1),
       (1, 'Laptops', true, 1),
       (2, 'Men’s Clothing', true, 2),
       (2, 'Women’s Clothing', true, 2);

INSERT INTO "catalog".items
(sub_category_id, "name", description, short_description, image_url, created_by)
VALUES (1, 'Samsung Galaxy S23',
        'The Samsung Galaxy S23 features a 6.1-inch Dynamic AMOLED 2X display, Snapdragon 8 Gen 2 processor, and a 50MP triple camera setup.',
        '6.1-inch AMOLED, 50MP camera, Snapdragon 8 Gen 2',
        'https://example.com/images/samsung-galaxy-s23.jpg',
        'admin'),
       (2, 'Apple iPhone 15 Pro',
        'The iPhone 15 Pro offers a titanium design, A17 Pro chip, and powerful 48MP camera system.',
        'A17 Pro chip, titanium body, 48MP camera',
        'https://example.com/images/iphone-15-pro.jpg',
        'admin'),
       (3, 'Levi’s 511 Slim Fit Jeans',
        'Modern slim fit jeans made from stretch denim for comfort and durability.',
        'Slim fit stretch denim jeans',
        'https://example.com/images/levis-511.jpg',
        'admin'),
       (4, 'Zara Floral Summer Dress',
        'Elegant floral dress made from breathable cotton fabric, perfect for summer.',
        'Cotton floral dress, breathable fabric',
        'https://example.com/images/zara-floral-dress.jpg',
        'admin');

INSERT INTO "catalog".units (id, unit_name, unit_order, conversion_ratio)
VALUES (1, 'Piece', 1, 1),
       (2, 'Kilogram', 2, 1),
       (3, 'Gram', 3, 0.001),
       (4, 'Litre', 4, 1),
       (5, 'Millilitre', 5, 0.001),
       (6, 'Pack', 6, 1),
       (7, 'Box', 7, 1),
       (8, 'Dozen', 8, 12);
SELECT setval(pg_get_serial_sequence('"catalog".units', 'id'), COALESCE(MAX(id), 1))
FROM "catalog".units;

INSERT INTO "catalog".stocks (item_id,
                              item_quantity,
                              rate,
                              rate_for,
                              initial_unit_id,
                              store_id,
                              is_active,
                              is_available,
                              reason,
                              sub_category_id,
                              discount_remarks,
                              discount_per,
                              is_discount_applicable)
VALUES (1, 250, 119999.99, 1, 1, 1, TRUE, TRUE, 'Initial stock upload', 1, NULL, 0.00, FALSE),
       (2, 150, 179999.99, 1, 1, 1, TRUE, TRUE, 'Initial stock upload', 2, NULL, 0.00, FALSE),
       (3, 500, 2999.99, 1, 1, 2, TRUE, TRUE, 'Initial stock upload', 3, NULL, 0.00, FALSE),
       (4, 400, 2499.99, 1, 1, 2, TRUE, TRUE, 'Initial stock upload', 4, NULL, 0.00, FALSE);
