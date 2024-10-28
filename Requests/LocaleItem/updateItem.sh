#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "ciao amore 34535",
    "context": "app",
    "userId": "0",
    "aggregateId": "5f0004cb-8b2f-4a6e-98c8-a3bcbf2349d2"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .



json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "ciao amore 88",
    "context": "appero",
    "userId": "0",
    "aggregateId": "5f0004cb-8b2f-4a6e-98c8-a3bcbf2349d2"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

