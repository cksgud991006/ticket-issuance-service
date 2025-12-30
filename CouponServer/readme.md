# Coupon Issuance Backend Service (Practice Project)

## Overview

This project is a backend practice service designed to issue a limited number of coupons while ensuring:

- Concurrency safety (no over-issuance)
- Idempotent request handling (duplicate requests do not create duplicate coupons)

The focus of this project is to practice on core backend engineering fundamentals rather than feature completeness.

---

## Goals of This Project

This project was created to practice and reinforce the following backend development concepts:

- REST API design
- Request handling
- Concurrency data access
- Backend architecture design
- C#
- .NET and EF Core

---

## High-Level Architecture

The application follows a layered architecture with clear separation of concerns:

API (Controller)
→ Application Service
→ Repository (Data Access)
→ Database

