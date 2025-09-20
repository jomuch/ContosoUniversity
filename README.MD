# Week 5 Revision – Student Forms

## Overview
This revision implements the **Create** and **Edit** functionality for the Student entity in the Contoso University application. Features include:

- **Model binding** via Razor Pages `[BindProperty]` and ViewModels.
- **Server-side and client-side validation** using Data Annotations and `_ValidationScriptsPartial`.
- **PRG (Post-Redirect-Get) pattern** to prevent duplicate submissions.
- **TempData success messages** to notify the user after successful creation or edits.

## Changes Made
- **Create.cshtml / Create.cshtml.cs**: Added form, ViewModel binding, validation, PRG, and TempData.
- **Edit.cshtml / Edit.cshtml.cs**: Updated to match Create page pattern with PRG and TempData.
- **Index.cshtml**: Added display for TempData success messages.

## Test Plan

### 1. Create Student

| Test Case | Steps | Expected Result |
|-----------|-------|----------------|
| Create with valid data | 1. Go to "Create Student" page.<br>2. Enter valid First Name, Last Name, and Enrollment Date.<br>3. Click "Create". | Student is saved in database.<br>Redirected to Index page.<br>Success message displays. |
| Create with missing required field | 1. Leave Last Name or First Name empty.<br>2. Click "Create". | Validation error displays next to empty field.<br>Student is not saved. |
| Create with invalid date | 1. Enter an invalid date format.<br>2. Click "Create". | Validation error displays for Enrollment Date. |
| Duplicate submission prevention (PRG) | 1. Submit the form.<br>2. Refresh Index page. | Student is not duplicated.<br>Success message appears only once. |

### 2. Edit Student

| Test Case | Steps | Expected Result |
|-----------|-------|----------------|
| Edit existing student with valid data | 1. Navigate to "Edit" page for a student.<br>2. Change fields.<br>3. Click "Save". | Changes are saved.<br>Redirected to Index.<br>Success message displays. |
| Edit with invalid data | 1. Remove required field or enter invalid date.<br>2. Click "Save". | Validation errors display.<br>No changes saved. |
| Edit non-existent student | 1. Navigate to `/Students/Edit?id=9999`. | Page returns 404 Not Found. |
| Duplicate submission prevention (PRG) | 1. Submit edits.<br>2. Refresh Index page. | Changes are not duplicated.<br>Success message appears only once. |

### 3. Navigation & UI Checks
- Back to List links on Create and Edit pages return to Index.
- Validation messages display correctly for all fields.
- Success messages appear only after successful POST.

## Instructions to Run
1. Open the project in Visual Studio or your preferred IDE.
2. Ensure the database is up to date using **EF Core migrations**.
3. Navigate to **Students → Create** or **Students → Edit** to test functionality.
4. Observe success messages and validation behavior.
