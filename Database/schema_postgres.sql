-- PostgreSQL schema for Loyalty API
CREATE TABLE IF NOT EXISTS "Members" (
    "Id" SERIAL PRIMARY KEY,
    "MobileNumber" VARCHAR(15) NOT NULL UNIQUE,
    "Name" VARCHAR(100),
    "IsVerified" BOOLEAN NOT NULL DEFAULT FALSE,
    "OtpCode" VARCHAR(10),
    "OtpExpiresAt" TIMESTAMP WITH TIME ZONE,
    "PointsBalance" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS "PointsTransactions" (
    "Id" SERIAL PRIMARY KEY,
    "MemberId" INTEGER NOT NULL REFERENCES "Members"("Id") ON DELETE CASCADE,
    "PointsChange" INTEGER NOT NULL,
    "Description" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS "CouponRedemptions" (
    "Id" SERIAL PRIMARY KEY,
    "MemberId" INTEGER NOT NULL REFERENCES "Members"("Id") ON DELETE CASCADE,
    "CouponCode" VARCHAR(64) NOT NULL,
    "PointsRedeemed" INTEGER NOT NULL,
    "CouponValue" NUMERIC(10,2) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Sample member (unverified). Use the API to verify.
INSERT INTO "Members" ("MobileNumber","Name","IsVerified","PointsBalance")
VALUES ('9999999999','Sample User', FALSE, 0)
ON CONFLICT DO NOTHING;
