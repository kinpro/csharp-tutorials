declare @date date = '2012-01-15'

;

-- Build a table of flags
with _flags as
(
	select
		contract_id,
		sum(case when event_type = 'LODE' then 1 else 0 end) disbursed,
		sum(case when event_type = 'LOCE' then 1 else 0 end) closed,
		sum(case when event_type = 'WROE' then 1 else 0 end) written_off
	from
		dbo.ContractEvents
	where
		is_deleted = 0
		and event_date <= @date
	group by
		contract_id
)
--select * from _flags

select 
	c.id, 
	c.contract_code
from
	dbo.Contracts c
left join
	_flags f on f.contract_id = c.id
where
	f.disbursed > 0
	and f.closed < 1
	and f.written_off < 1