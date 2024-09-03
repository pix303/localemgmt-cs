#!/bin/bash

json_data=$(
  cat <<EOF
{
    "firstname": "paolo",
    "lastname": "di giorgio",
    "email": "paolo@ciao.it",
    "role": 0
}
EOF
)

curl POST http://localhost:5028/user/register/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .
