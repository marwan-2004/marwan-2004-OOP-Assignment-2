using SrpLab;

Console.WriteLine("SrpLab — 10 intentional SRP violations (refactor me)");
Console.WriteLine("=================================================");

// 1. WardBoard
var ward = new WardBoard();
var handoffFormatter = new NurseHandoffNoteFormatter();
ward.AssignBed(1, "p-88", heartRate: 130, spo2: 89);
Console.WriteLine(handoffFormatter.BuildHandoffNote(ward, 1));
Console.WriteLine(string.Join(" | ", ward.DrainPagerLog()));

// 2. CheckoutBasket
var basket = new CheckoutBasket();
var paymentAuthorizer = new PaymentAuthorizerStub();
basket.AddLine("SKU-1", 40m, 2);
basket.ApplyCouponText("SAVE10");
basket.EnableGiftWrap();
Console.WriteLine($"basket total={basket.GrandTotal()} auth={paymentAuthorizer.AuthorizePayment(basket, "4242")}");

// 3. SupportTicket
var ticket = new SupportTicket("T-1", "cannot login", "prod is down for me", DateTimeOffset.UtcNow);
var replyDraftsman = new PublicReplyDraftsman();
Console.WriteLine(replyDraftsman.DraftPublicReply(ticket, "Nora"));

// 4. LoanDesk
var loan = new LoanDesk(60_000m, 640, 4, hasCollateral: false);
var decisionLetterFormatter = new LoanDecisionLetterFormatter();
Console.WriteLine(decisionLetterFormatter.DecisionLetter(loan, "Omar"));

// 5. CourseEnrollmentDesk
var course = new CourseEnrollmentDesk("SEF-101", capacity: 1, tuition: 3000m);
var welcomeFormatter = new EnrollmentWelcomePacketFormatter();
Console.WriteLine(course.Register("a@mail.com"));
Console.WriteLine(course.Register("b@mail.com"));
Console.WriteLine(welcomeFormatter.GenerateMarkdown(course, "b@mail.com", "Bea"));

// 6. KitchenTicket
var kitchen = new KitchenTicket();
var thermalRenderer = new ThermalTicketRenderer();
kitchen.AddItem("Pasta", new[] { "wheat", "milk" }, 12);
Console.WriteLine(thermalRenderer.RenderThermalTicket(kitchen, 42));

// 7. SubscriptionBilling
var sub = new SubscriptionBilling("c-9", 99m, new DateOnly(2026, 9, 1), new DateOnly(2026, 10, 1));
var invoiceGen = new InvoiceNumberGenerator();
var dunningComposer = new DunningEmailComposer(invoiceGen);
sub.RegisterFailedPayment();
Console.WriteLine(dunningComposer.DunningEmail(sub, "Sara", new DateOnly(2026, 9, 20)));

// 8. WarehousePickList
var pick = new WarehousePickList();
var pickerFormatter = new HandheldPickerScriptFormatter();

pick.AddNeed("BOLT", "A", 3, 10, 7);
pick.AddNeed("NUT", "B", 1, 5, 5);

Console.WriteLine(pickerFormatter.PickerScript(pick));

// 9. GradeBook
var grades = new GradeBook();
var transcriptFormatter = new TranscriptFormatter();
grades.Record("s1", 92);
grades.Record("s1", 88);
Console.WriteLine(transcriptFormatter.FormatPlain(grades, "s1", "Ali"));

// 10. AppointmentDesk
var appt = new AppointmentDesk(new TimeOnly(9, 0), new TimeOnly(17, 0), 30);
var smsNotifier = new AppointmentSmsNotifier();
var slot = appt.FindNextSlot(DateTimeOffset.Parse("2026-09-21T08:00:00Z"), 48);
if (slot is null) throw new InvalidOperationException("no slot");
appt.TryBook(slot.Value);
Console.WriteLine(smsNotifier.SmsReminder(slot.Value, "0100"));

Console.WriteLine("Done. Now split responsibilities — without breaking behavior.");