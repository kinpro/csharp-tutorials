-- Running total: raw
select 
	a.number,
	a.capital_repayment
from
	dbo.Installments a
inner join
	dbo.Installments b on a.contract_id = b.contract_id and a.number >= b.number
where
	a.contract_id = 1
order by
	a.contract_id, a.number

-- Running total: grouped
select 
	a.number,
	a.capital_repayment,
	sum(b.capital_repayment) running_total
from
	dbo.Installments a
inner join
	dbo.Installments b on a.contract_id = b.contract_id and a.number >= b.number
where
	a.contract_id = 1
group by
	a.contract_id, a.number, a.capital_repayment
order by
	a.contract_id, a.number