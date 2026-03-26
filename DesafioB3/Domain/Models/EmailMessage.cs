using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Domain.Models
{
    static class EmailMenssage
    {
        public static string defaultSubject = "Trading Opportunity - [Asset Name]";

        public static string defaultMenssageemailBuy =
            "Dear [User's Name]," +
            "\r\n\r\nWe hope this email finds you well. We're reaching out to inform you that the [Asset Name] " +
            "has reached a significant value, presenting a unique opportunity in the market. Based on our market " +
            "analysis, we'd like to share some recommendations: " +
            "Your asset has reached a value indicating a potential buying opportunity. We recommend considering this " +
            "option as there are signs of potential future growth. Please remember to conduct your own research and " +
            "analysis before making any investment decisions. " +
            "We emphasize the importance of consulting with your financial advisor before executing any transactions. " +
            "The financial market is dynamic, and conditions can change rapidly.\r\n\r\nThank you for trusting our analyses, " +
            "and we are available to provide more information if needed. We wish you success in your investment decisions." +
            "\r\n\r\nBest regards,\r\n\r\n[Your Name or Company Name]\r\n[Your Position]\r\n[Contact Information]";

        public static string defaultMenssageemailSell =
            "Dear [User's Name]," +
            "\r\n\r\nWe hope this email finds you well. We're reaching out to inform you that the [Asset Name] " +
            "has reached a significant value, presenting a unique opportunity in the market. Based on our market " +
            "analysis, we'd like to share some recommendations: " +
            "The current value of the [Asset Name] also suggests an opportunity for selling. If you're looking to realize" +
            " profits or adjust your portfolio, this may be a strategic moment to sell part or all of your position." +
            "We emphasize the importance of consulting with your financial advisor before executing any transactions. " +
            "The financial market is dynamic, and conditions can change rapidly.\r\n\r\nThank you for trusting our analyses, " +
            "and we are available to provide more information if needed. We wish you success in your investment decisions." +
            "\r\n\r\nBest regards,\r\n\r\n[Your Name or Company Name]\r\n[Your Position]\r\n[Contact Information]";
    }
}
