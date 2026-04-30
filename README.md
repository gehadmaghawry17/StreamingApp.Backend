# StreamingApp.Backend
# StreamView — Project Requirements

> ASP.NET Core 9 · Clean Architecture · EF Core Code-First 
---

## Project overview

StreamView is a backend REST API for a movie and series streaming platform.  
It handles authentication, content management, subscription billing, user engagement, and offline downloads.

**One rule guides every design decision:**  
constraints and business rules live in the schema — not scattered across application code.

---

## Stats at a glance
| Feature modules | 6 (Auth, Content, Engagement, Subscription, Downloads, Platform) |
| Authentication methods | 3 (Password, OTP, PIN) |

---

## Functional requirements

### Module 1 — Authentication & users

| ID | Requirement | Detail |
|---|---|---|
| FR-01 | User registration with email and password | Password stored as bcrypt hash (work factor 12). Email unique-indexed. |
| FR-02 | OTP verification on signup and forgot-password | 6-digit code, expires 5 min, max 3 attempts, bcrypt-hashed, single-use. |
| FR-03 | JWT access + refresh token authentication | Access: 1-hour expiry. Refresh: 7-day expiry, SHA-256 hash in DB, rotated on every use. |
| FR-04 | Optional PIN for quick login | bcrypt-hashed. Locks after 3 failed attempts for 5 minutes. |
| FR-05 | One account per user — no profile switching | Single User entity. Email is unique-indexed. No profile collection. |
| FR-06 | User preferences and settings | Language, subtitle, video quality, autoplay, notifications. 1-to-1 with User. |

---

### Module 2 — Content

| ID | Requirement | Detail |
|---|---|---|
| FR-07 | Movies — trailer free, full video requires subscription | `TrailerUrl` public. `VideoUrl` subscription-gated. Enforced in service layer. |
| FR-08 | Series with seasons and episodes | Series → Season → Episode hierarchy. First 2 episodes: `IsFreePreview = true`. |
| FR-09 | Genre classification for movies and series | Many-to-many via `MovieGenre` and `SeriesGenre`. Composite unique index on each. |
| FR-10 | Global search with search log tracking | Every search recorded in `SearchLog` (query + result count) for analytics. |

---

### Module 3 — User engagement

| ID | Requirement | Detail |
|---|---|---|
| FR-11 | Favorites — add movie or series to watchlist | Polymorphic: `ContentType` + `ContentId`. Composite unique index per user per content. |
| FR-12 | Ratings — 1 to 5 star score per content | Polymorphic. One rating per user per content. Average cached on Movie/Series. |
| FR-13 | Reviews — written review per content item | Polymorphic. One review per user per content. Optional rating score alongside text. |
| FR-14 | Viewing progress — resume where you left off | Tracks `ProgressSeconds` + `TotalDurationSeconds`. `IsCompleted` auto-calculated (≥ 90%). |
| FR-15 | Watch history — full viewing log | No unique constraint — same content can appear multiple times in history. |
| FR-16 | Genre interests — personalization seed | User selects preferred genres. Many-to-many via `UserGenreInterest`. |

---

### Module 4 — Subscription & payment

| ID | Requirement | Detail |
|---|---|---|
| FR-17 | Subscription plans with configurable tiers | Defines: Price, BillingCycle, MaxDownloads, MaxConcurrentStreams. `IsActive` flag. |
| FR-18 | User subscription lifecycle | Status: PendingPayment → Active → Cancelled / Expired. AutoRenew. StartDate + EndDate. |
| FR-19 | Payment transaction audit trail | Every attempt recorded: Amount, Currency, Status, TransactionReference, PaidAt. |

---

### Module 5 — Downloads

| ID | Requirement | Detail |
|---|---|---|
| FR-20 | Offline download metadata tracking | Status: Pending → InProgress → Completed / Failed / Expired. FilePath, FileSizeBytes, ExpiresAt. |

---

### Module 6 — Platform

| ID | Requirement | Detail |
|---|---|---|
| FR-21 | Notifications system | Types: NewContent, SubscriptionExpiring, PaymentFailed, NewEpisode, General. IsRead + ReadAt. |

---

## Non-functional requirements

| ID | Requirement | Implementation |
|---|---|---|
| NFR-01 | **Security** — all secrets hashed, never stored raw | Passwords: bcrypt 12 · PINs: bcrypt 10 · OTPs: bcrypt 10 · Refresh tokens: SHA-256 · JWT: HS256 |
| NFR-02 | **Maintainability** — Clean Architecture with strict layer separation | Domain has zero external dependencies. Infrastructure is the only EF Core / JWT layer. |
| NFR-03 | **Data integrity** — constraints at database level | Unique indexes, composite keys, cascade/restrict delete behaviors — not just app-level validation. |
| NFR-04 | **Performance** — query-optimized indexes | Indexes on UserId, ContentType+ContentId, Status, ExpiresAt, SearchQuery+SearchedAt. |
| NFR-05 | **Scalability** — UUID primary keys throughout | No sequential integers. Supports distributed ID generation. No enumeration risk on public endpoints. |
| NFR-06 | **Auditability** — soft delete + timestamps on every entity | BaseEntity: CreatedAt, UpdatedAt, IsDeleted, DeletedAt. Nothing is permanently removed. |
| NFR-07 | **Extensibility** — polymorphic tables for future content types | `ContentType` enum can add new values without schema migration on Favorite / Rating / Review. |
| NFR-08 | **Testability** — interfaces + dependency injection throughout | Every service depends on an interface. Repositories are injected. Domain has no static dependencies. |
| NFR-09 | **Documentation** — Swagger / OpenAPI on all endpoints | All controllers and endpoints annotated. Request/response schemas auto-generated. |

---

## Out of scope — version 1

- Real payment gateway integration (Stripe, PayPal)
- Video transcoding or CDN delivery
- Admin dashboard
- Social features (follow, share, comments)
- Recommendation algorithm
- Multi-language content metadata

---

## Assumptions

- One user = one account. No shared or child profiles.
- Video files are hosted externally. The database stores URLs only.
- OTP delivery is handled by an external email/SMS service.
- Subscription access is checked per request in the service layer.
- Concurrent stream limit is enforced in the service layer using active session count.
- All timestamps are stored and returned as UTC.

---

## Constraints summary — encoded in the schema

| Rule | Schema enforcement |
|---|---|
| One PIN per user | `UNIQUE(UserId)` on `UserPins` |
| One preference record per user | `UNIQUE(UserId)` on `UserPreferences` |
| One genre interest per user-genre pair | `UNIQUE(UserId, GenreId)` on `UserGenreInterests` |
| One rating per user per content | `UNIQUE(UserId, ContentType, ContentId)` on `Ratings` |
| One review per user per content | `UNIQUE(UserId, ContentType, ContentId)` on `Reviews` |
| One favorite per user per content | `UNIQUE(UserId, ContentType, ContentId)` on `Favorites` |
| Season number unique within a series | `UNIQUE(SeriesId, SeasonNumber)` on `Seasons` |
| Episode number unique within a season | `UNIQUE(SeasonId, EpisodeNumber)` on `Episodes` |
| Subscription plan cannot be deleted while active | `ON DELETE RESTRICT` on `SubscriptionPlan → UserSubscription` |

---

*Built by [Your Name] · [LinkedIn] · [GitHub]*
