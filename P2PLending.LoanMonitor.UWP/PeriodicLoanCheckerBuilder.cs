//using System;

//namespace PaskoluKlubas.UWP.NewLoanWatcher
//{
//    public interface IPeriodicLoanCheckerBuilder
//    {
//        PeriodicLoanChecker Build();
//    }

//    public interface IPeriodicLoanCheckerBuilderTimespanSetup
//    {
//        IPeriodicLoanCheckerBuilderHandlerSetup CheckEvery(TimeSpan span);
//    }

//    public interface IPeriodicLoanCheckerBuilderHandlerSetup
//    {
//        IPeriodicLoanCheckerBuilder CallOnNewLoans(Action<LoanListing> handler);
//    }

//    public class PeriodicLoanCheckerBuilder : IPeriodicLoanCheckerBuilder, IPeriodicLoanCheckerBuilderTimespanSetup, IPeriodicLoanCheckerBuilderHandlerSetup
//    {
//        private readonly LoanListMonitor _monitor;
//        private readonly TimeSpan _timeSpan;
//        private readonly Action<LoanListing> _handler;
        
//        private PeriodicLoanCheckerBuilder(
//            LoanListMonitor monitor,
//            TimeSpan timeSpan,
//            Action<LoanListing> handler) : this(monitor, timeSpan)
//        {
//            _handler = handler;
//        }

//        private PeriodicLoanCheckerBuilder(LoanListMonitor monitor, TimeSpan timeSpan) : this(monitor)
//        {
//            _timeSpan = timeSpan;
//        }

//        private PeriodicLoanCheckerBuilder(LoanListMonitor monitor)
//        {
//            _monitor = monitor;
//        }
        
//        public static IPeriodicLoanCheckerBuilderTimespanSetup SetLoanIssuer(LoanIssuerClientConfiguration cfg)
//        {
//            var monitor = LoanListMonitorFactory.Create(cfg);
            
//            return new PeriodicLoanCheckerBuilder(monitor);
//        }

//        public IPeriodicLoanCheckerBuilderHandlerSetup CheckEvery(TimeSpan span)
//        {
//            return new PeriodicLoanCheckerBuilder(_monitor, span);
//        }

//        public PeriodicLoanChecker Build()
//        {
//            return new PeriodicLoanChecker(_monitor, _timeSpan, _handler);
//        }

//        public IPeriodicLoanCheckerBuilder CallOnNewLoans(Action<LoanListing> handler)
//        {
//            return new PeriodicLoanCheckerBuilder(_monitor, _timeSpan, handler);
//        }
//    }
//}
