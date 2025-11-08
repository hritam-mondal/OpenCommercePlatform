-- ---------------------------------------------------------------------
-- FUNCTION: register_user()
-- ---------------------------------------------------------------------

/*
    Function: register_user
    Description:
        Registers a new user if the email does not already exist, 
        incorporating required fields from the updated users table.

    Parameters:
        p_username      - User's unique username (VARCHAR(50) NOT NULL)
        p_first_name    - User's first name (VARCHAR(50) NOT NULL)
        p_last_name     - User's last name (VARCHAR(50) NOT NULL)
        p_email         - User's email (VARCHAR(254) NOT NULL UNIQUE)
        p_phone_number  - User's phone number (VARCHAR(25))
        p_password_hash - Hashed password (VARCHAR(255) NOT NULL)
        p_password_salt - Salt (VARCHAR(255))
        p_token         - Email confirmation token (UUID)
        p_expires       - Expiration time for token (TIMESTAMP)

    Returns:
        1 - Registration successful
        0 - Email already exists
*/

CREATE
OR REPLACE FUNCTION users.register_user(p_username character varying, p_first_name character varying,
                                            p_last_name character varying, p_email character varying,
                                            p_phone_number character varying, p_password_hash character varying,
                                            p_password_salt character varying, p_token uuid,
                                            p_expires timestamp with time zone)
    RETURNS integer
    LANGUAGE plpgsql
AS
$function$
BEGIN
    -- Check if a user with the provided email already exists
    IF
EXISTS (SELECT 1
                FROM users.users
                WHERE email = p_email) THEN
        RETURN 0; -- Email already exists, registration failed
END IF;

    -- Insert new user record into the users table
INSERT INTO users.users (username,
                         first_name,
                         last_name,
                         email,
                         phone_number,
                         password_hash,
                         password_salt,
                         email_confirm_token,
                         email_confirm_token_expires)
VALUES (p_username,
        p_first_name,
        p_last_name,
        p_email,
        p_phone_number,
        p_password_hash,
        p_password_salt,
        p_token,
        p_expires);

RETURN 1; -- Registration successful
END;
$function$
;
-- Set function owner and grant execute permissions
ALTER FUNCTION users.register_user(varchar, varchar, varchar, varchar, varchar, varchar, varchar, uuid, timestamptz) OWNER TO ocp_user;
GRANT
ALL
ON FUNCTION users.register_user(varchar, varchar, varchar, varchar, varchar, varchar, varchar, uuid, timestamptz) TO public;
GRANT ALL
ON FUNCTION users.register_user(varchar, varchar, varchar, varchar, varchar, varchar, varchar, uuid, timestamptz) TO ocp_user;

---

-- ---------------------------------------------------------------------
-- FUNCTION: users.get_user_by_id(p_user_id integer)
-- ---------------------------------------------------------------------

/*
    Function: get_user_by_id
    Description:
        Fetches all columns for a single user record based on the internal user_id.

    Parameters:
        p_user_id - The internal primary key ID of the user (integer).

    Returns:
        TABLE - All columns from the users.users table matching the ID.
*/
CREATE
OR REPLACE FUNCTION users.get_user_by_id(p_user_id integer)
    RETURNS TABLE
            (
                user_id                     integer,
                uuid                        uuid,
                username                    character varying,
                first_name                  character varying,
                last_name                   character varying,
                email                       character varying,
                phone_number                character varying,
                country_code                character,
                password_hash               text,
                password_salt               text,
                password_reset_token        uuid,
                reset_token_expires_at      timestamp with time zone,
                is_active                   boolean,
                is_admin                    boolean,
                is_confirmed                boolean,
                email_confirm_token         uuid,
                email_confirm_token_expires timestamp with time zone,
                created_at                  timestamp with time zone,
                updated_at                  timestamp with time zone,
                last_login_at               timestamp with time zone
            )
    LANGUAGE sql -- Use SQL language for simple SELECT query
AS
$$
-- Selects all columns necessary for the return table type
SELECT user_id,
       uuid,
       username,
       first_name,
       last_name,
       email,
       phone_number,
       country_code,
       password_hash,
       password_salt,
       password_reset_token,
       reset_token_expires_at,
       is_active,
       is_admin,
       is_confirmed,
       email_confirm_token,
       email_confirm_token_expires,
       created_at,
       updated_at,
       last_login_at
FROM users.users
WHERE user_id = p_user_id; -- Filter by the provided user ID
$$;
-- Set function owner
ALTER FUNCTION users.get_user_by_id(integer) OWNER TO ocp_user;
GRANT
ALL
ON FUNCTION users.get_user_by_id(integer) TO ocp_user;
GRANT EXECUTE ON FUNCTION users.get_user_by_id
(integer) TO public; -- Typically granted for read functions

---

-- ---------------------------------------------------------------------
-- FUNCTION: get_auth_user(p_login text)
-- ---------------------------------------------------------------------

/*
    Function: get_auth_user
    Description:
        Fetches a user's essential authentication fields by email or username.
        Used primarily during the login process.

    Parameters:
        p_login  - Email or Username (case-insensitive)

    Returns:
        TABLE of user_id, username, email, password_hash, password_salt,
              is_active, is_confirmed, first_name, last_name, is_admin (authentication essentials)
*/

CREATE
OR REPLACE FUNCTION users.get_auth_user(p_login text)
    RETURNS TABLE
            (
                user_id       bigint,
                username      character varying,
                email         character varying,
                password_hash text,
                password_salt text,
                is_active     boolean,
                is_confirmed  boolean,
                first_name    character varying,
                last_name     character varying,
                is_admin      boolean
            )
    LANGUAGE plpgsql
AS
$function$
BEGIN
    -- Return the result of the query
RETURN QUERY
SELECT u.user_id,
       u.username,
       u.email,
       u.password_hash,
       u.password_salt,
       u.is_active,
       u.is_confirmed,
       u.first_name,
       u.last_name,
       u.is_admin
FROM users.users u
-- Case-insensitive search on both email and username
WHERE LOWER(u.email) = LOWER(p_login)
   OR LOWER(u.username) = LOWER(p_login) LIMIT 1; -- Ensures only one user is returned even if multiple matches are somehow found
END;
$function$
;
-- Set function owner and grant execute permissions
ALTER FUNCTION users.get_auth_user(text) OWNER TO ocp_user;
GRANT
ALL
ON FUNCTION users.get_auth_user(text) TO ocp_user;
GRANT EXECUTE ON FUNCTION users.get_auth_user
(text) TO public;

---

-- ---------------------------------------------------------------------
-- FUNCTION: users.get_users (Paginated List)
-- ---------------------------------------------------------------------

/*
    Function: get_users
    Description:
        Retrieves a paginated list of users from the "users" table
        with optional filtering by search query (username/email) and active status.

    Parameters:
        p_query     - Optional search string to filter by username or email (case-insensitive).
                      If NULL, all users are returned.
        p_is_active - Optional boolean filter for active users.
                      If NULL, both active and inactive users are returned.
        p_limit     - Maximum number of records to return (default: 100, min 1, max 1000).
        p_offset    - Number of records to skip for pagination (default: 0, min 0).

    Returns:
        TABLE - All columns from the users.users table, filtered and paginated.

    Notes:
        - Uses ILIKE for case-insensitive search on username and email.
        - Pagination is clamped to safe limits using GREATEST/LEAST.
*/

CREATE
OR REPLACE FUNCTION users.get_users(
    p_query text DEFAULT NULL::text,
    p_is_active boolean DEFAULT NULL::boolean,
    p_limit integer DEFAULT 100,
    p_offset integer DEFAULT 0
)
    RETURNS TABLE
            (
                user_id                     bigint,
                uuid                        uuid,
                username                    character varying,
                first_name                  character varying,
                last_name                   character varying,
                email                       character varying,
                phone_number                character varying,
                country_code                character,
                password_hash               text,
                password_salt               text,
                password_reset_token        uuid,
                reset_token_expires_at      timestamp with time zone,
                is_active                   boolean,
                is_admin                    boolean,
                is_confirmed                boolean,
                email_confirm_token         uuid,
                email_confirm_token_expires timestamp with time zone,
                created_at                  timestamp with time zone,
                updated_at                  timestamp with time zone,
                last_login_at               timestamp with time zone
            )
    LANGUAGE plpgsql
AS
$function$
BEGIN
    -- Return the query result
RETURN QUERY
SELECT u.user_id,
       u.uuid,
       u.username,
       u.first_name,
       u.last_name,
       u.email,
       u.phone_number,
       u.country_code,
       u.password_hash,
       u.password_salt,
       u.password_reset_token,
       u.reset_token_expires_at,
       u.is_active,
       u.is_admin,
       u.is_confirmed,
       u.email_confirm_token,
       u.email_confirm_token_expires,
       u.created_at,
       u.updated_at,
       u.last_login_at
FROM users.users u
WHERE
  -- Filter by search query (username or email) using case-insensitive LIKE (ILIKE)
    (p_query IS NULL OR u.username ILIKE ('%' || p_query || '%') OR u.email ILIKE ('%' || p_query || '%'))
  -- Filter by active status (if p_is_active is provided)
  AND (p_is_active IS NULL OR u.is_active = p_is_active)
ORDER BY u.user_id DESC -- Order by ID descending for newest users first
         -- Apply pagination: limit clamped between 1 and 1000, offset clamped to minimum 0
    LIMIT GREATEST(1, LEAST(p_limit, 1000))
OFFSET GREATEST(0, p_offset);
END;
$function$;

-- Permissions

-- Set function owner
ALTER FUNCTION users.get_users(text, bool, int4, int4) OWNER TO ocp_user;

-- Grant execute permission to the owner
GRANT
ALL
ON FUNCTION users.get_users(text, bool, int4, int4) TO ocp_user;
GRANT EXECUTE ON FUNCTION users.get_users
(text, bool, int4, int4) TO public;

-- Creates a function in the 'audit' schema named 'get_audit_logs'
-- It accepts one integer parameter: _limit
-- It returns a table with a structure matching our C# DTO

CREATE OR REPLACE FUNCTION audit.get_audit_logs(_limit INT)
RETURNS TABLE (
    id int,
    ref_id varchar,
    user_identifier varchar,
    activity text,
    activity_date timestamp with time zone
) AS $$
BEGIN
    -- This RETURN QUERY statement executes the query and returns its result set
RETURN QUERY
SELECT
    a.id,
    a.ref_id,
    a.user_identifier,
    a.activity,
    a.activity_date
FROM
    audit.audit_logs AS a
ORDER BY
    a.id DESC
    LIMIT
        _limit;
END;
$$ LANGUAGE plpgsql;