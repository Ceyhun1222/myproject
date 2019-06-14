-- Table: "EventConsistency"

-- DROP TABLE "EventConsistency";

CREATE TABLE "EventConsistency"
(
  id serial NOT NULL,
  repositorytype integer,
  storagename character varying(64),
  workpackage integer,
  featuretype integer,
  identifier uuid,
  interpretation integer,
  sequencenumber integer,
  correctionnumber integer,
  validtimebegin timestamp without time zone,
  validtimeend timestamp without time zone,
  submitdate timestamp without time zone,
  hash character varying(128),
  calculationdate timestamp without time zone,
  CONSTRAINT "EventConsistency_pkey" PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "EventConsistency"
  OWNER TO aran;
