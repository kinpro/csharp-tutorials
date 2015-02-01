if not object_id('dbo.TutGetOlb') is null
drop function dbo.TutGetOlb
go

create function dbo.TutGetOlb(@loanId int)
returns money
as begin
	declare @result money
	select 
		@result = sum(capital_repayment - paid_capital)
	from
		dbo.Installments
	where
		contract_id = @loanId

	return @result
end