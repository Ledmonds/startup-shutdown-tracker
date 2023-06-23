namespace startup_shutdown_tracker.Application.Calculators.Presentation;

public record Calculation(
	MetricCalculation Total,
	MetricCalculation Monthly,
	MetricCalculation Weekly
);