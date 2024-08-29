#!/bin/bash

json_data=$(
  cat <<EOF
{
    "firstname": "paola",
    "lastname": "paolaaaa",
    "email": "paolo@ciao.it",
    "role": 0
}
EOF
)

curl POST http://localhost:5028/user/register/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  > post_result.json