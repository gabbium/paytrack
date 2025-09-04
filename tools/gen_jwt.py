"""
Simple JWT generator for Dev/Test (HS256).
- Reads secret from --secret.
- Sets iss, aud, sub, email and custom claims.
- Prints the compact token on stdout (single line).
"""

import argparse
import jwt
from datetime import datetime, timedelta, timezone


def main():
    p = argparse.ArgumentParser(description="Generate a HS256 JWT for Dev/Test.")
    p.add_argument("--secret", required=True, help="Signing secret (HS256).")
    p.add_argument("--issuer", default="paytrack", help="Token issuer (iss).")
    p.add_argument("--audience", default="paytrack", help="Token audience (aud).")
    p.add_argument(
        "--sub",
        required=True,
        help="Subject/user id (sub).",
    )
    p.add_argument(
        "--ttl-hours",
        type=int,
        default=24,
        help="Token lifetime in hours (default: 24).",
    )
    p.add_argument(
        "--out",
        default=".tmp/token.txt",
        help="File to write the token (default: token.txt).",
    )
    args = p.parse_args()

    now = datetime.now(timezone.utc)
    exp = now + timedelta(hours=args.ttl_hours)

    claims = {
        "iss": args.issuer,
        "aud": args.audience,
        "sub": args.sub,
        "iat": int(now.timestamp()),
        "nbf": int(now.timestamp()),
        "exp": int(exp.timestamp()),
    }

    token = jwt.encode(claims, args.secret, algorithm="HS256")

    print(token)

    with open(args.out, "w", encoding="utf-8") as f:
        f.write(token)


if __name__ == "__main__":
    main()
