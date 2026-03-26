# Stock Quote Alert

Stock Quote Alert is a C# console application that monitors the price of a B3 stock and sends an email notification when the price reaches a buy or sell threshold.

## Features

- Monitors a stock from B3
- Sends a **buy alert** when the price goes below the buy threshold
- Sends a **sell alert** when the price goes above the sell threshold
- Avoids repeated emails while the price remains in the same alert state
- Supports multiple quote providers
- Includes integration tests for email alert scenarios

## Requirements

- .NET SDK installed
- Internet access
- A valid SMTP account for sending emails
- API keys for the quote providers used by the project

## Project Configuration

The application uses an `appsettings.json` file for API and email configuration.

Create or update the file with values like the example below:

```json
{
  "ApiKeys": {
    "AlphaVantage": "YOUR_ALPHA_VANTAGE_KEY",
    "FinancialModel": "YOUR_FINANCIAL_MODELING_PREP_KEY"
  },
  "Email": {
    "SenderEmail": "your-email@example.com",
    "RecipientEmail": "destination-email@example.com",
    "SmtpHost": "smtp.example.com",
    "SmtpPort": 587,
    "Password": "YOUR_EMAIL_PASSWORD_OR_APP_PASSWORD"
  }
}