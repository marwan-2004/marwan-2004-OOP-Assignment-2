1\. AppointmentDesk

Identified Responsibilities:Appointment Scheduling \& Availability: Managing booked slots and finding available appointment times within business hours.   Calendar Export Formatting: Serializing booked slots into iCalendar (ICS) string format.   SMS Notification Formatting: Constructing text messaging reminder strings for patients.  



&#x20;Why Coupling Is Bad:

Combining slot booking logic with presentation/interoperability formats (ICS and SMS) means changing the SMS wording or upgrading calendar export standards forces re-testing and re-deploying core scheduling rules.



2\. CheckoutBasket

Identified Responsibilities:Cart State \& Total Calculation: Managing line items, gift wrapping flags, subtotal, and grand total logic.   Coupon Code Parsing: Parsing marketing promo codes and calculating discount percentages/amounts.   Gift Messaging Formatting: Generating customer-facing gift message cards.   Payment Gateway Authorization Stub: Generating authorization payload hashes for payment processing.



&#x20;Why Coupling Is Bad:

Marketing coupon rule changes or changes to the payment provider auth protocol directly touch the core shopping cart data structure.



3\. CourseEnrollmentDesk

Identified Responsibilities:Course Capacity \& Waitlist Management: Managing seated students, waitlist positioning, and waitlist promotion.   Welcome Packet Formatting: Rendering Markdown onboarding packets.   Invoice Line Formatting: Calculating VAT and generating invoice line strings for finance.   



Why Coupling Is Bad:

Updating billing format rules or changing Discord/Markdown links forces modifications to the seat availability and waitlist tracking class.



4\. GradeBook

Identified Responsibilities:Score Recording \& Mathematical Aggregation: Storing student scores and calculating numerical averages.   Academic Evaluation Policy: Applying grading scale letters (A, B, C...) and honor roll status business logic.   Transcript Document Formatting: Generating formatted plain text transcripts.   Data Export Formatting: Serializing the grade book records into CSV format.  



&#x20;Why Coupling Is Bad:

Academic policy decisions (e.g., changing grade boundary thresholds or honor roll criteria) or export adjustments (e.g., modifying CSV header columns) are mixed directly with basic score storage and math operations.



5\. KitchenTicket

Identified Responsibilities:Kitchen Ticket \& Queue Management: Storing item prep times and ingredients, and calculating kitchen queue prep time heuristics.   Allergen Detection: Scanning ingredient lists against regulatory allergen keyword dictionaries.   Thermal Printer Ticket Formatting: Formatting hardware-specific ticket layouts with fixed widths and separators.   Expo Lane Dispatch Routing: Determining dispatch lane tags based on prep rules and allergen checks. 

&#x20; 

Why Coupling Is Bad:

Changing printer hardware format widths or updating regulatory allergen definitions directly affects core kitchen queue time algorithms.





6\. LoanDesk

Identified Responsibilities:Risk Scoring \& Loan Eligibility: Computing credit risk score algorithms and determining loan eligibility.   Compliance Document Checklisting: Determining mandatory document checklists based on regulatory rules.   Decision Letter Formatting: Drafting pre-approval and rejection prose for applicants.   Underwriter Export Formatting: Formatting applicant data into CSV strings for underwriter analytics.  



&#x20;Why Coupling Is Bad:

Updating regulatory checklist requirements or changing letter wording forces changes in the core risk scoring calculation engine



7\. SubscriptionBilling

Identified Responsibilities:Subscription State \& Proration Math: Managing subscription accounts and calculating period prorated amounts.   Invoice Number Generation: Minting sequential invoice number strings with fiscal prefixes.   Dunning Email Formatting: Constructing collection email templates and severity subject lines.   Ledger Export Formatting: Formatting accounting ledger journal lines.  



&#x20;Why Coupling Is Bad:

Drafting dunning emails currently causes hidden side-effects by incrementing global invoice numbers while composing text. 



8\. SupportTicket

Identified Responsibilities:Ticket State \& SLA Calculation: Managing ticket lifecycle and evaluating SLA deadline breaches.   Text Priority Classification: Analyzing customer message text to assign urgency priority levels (P1/P2/P3).   Customer Reply Formatting: Drafting public response templates based on incident priority.   Internal Escalation Alert Formatting: Generating escalation logs for internal ops.  



&#x20;Why Coupling Is Bad:

Adjusting keyword heuristics or customer reply templates alters the class responsible for core SLA enforcement.



9\. WardBoard

Identified Responsibilities:Hospital Ward Bed Tracking: Managing bed assignments and patient mapping.   Clinical Acuity Evaluation: Computing acuity scores based on vital signs (HR, SpO2).   Emergency Pager Logging: Managing emergency code notifications and pager logs.   Nurse Handoff Note Formatting: Formatting handoff notes for medical staff.   Census Data Exporting: Exporting bed census state to CSV format.  

&#x20;Why Coupling Is Bad:



Mixing vital sign clinical logic, emergency paging, and census export means a change in report formatting could accidentally disrupt patient tracking or critical code paging.



10\. WarehousePickList

Identified Responsibilities:Stock Allocation Logic: Calculating stock allocation vs. backorders.   Warehouse Walking Path Optimization: Ordering picking locations by aisle and bin.   Handheld Picker UI Scripting: Formatting human-readable picker instruction step strings.   WMS XML Integration: Serializing batch allocations into WMS XML format.  



&#x20;Why Coupling Is Bad:

Changes to the XML integration contract or handheld UI copy directly affect warehouse route optimization and inventory allocation.





