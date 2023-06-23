namespace startup_shutdown.calculation.application.Calculators.Presentation;

public record Calculation(
    MetricCalculation Total,
    MetricCalculation Monthly,
    MetricCalculation Weekly
);
